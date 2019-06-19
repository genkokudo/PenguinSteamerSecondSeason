using Microsoft.Extensions.Logging;
using PenguinSteamerSecondSeason.Data;
using PenguinSteamerSecondSeason.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PenguinSteamerSecondSeason.Common
{
    /// <summary>
    /// Tickerからローソク足データを作成する
    /// 取引所と時間足ごとにこのクラスオブジェクトを作成する
    /// このクラスは親子関係を持ち、複数の子を持つ場合もある
    /// 1分足が1番親になる
    /// 親が更新したら、子に更新が伝搬する
    /// 
    /// 登録間隔は検討する → 取り敢えず新しくローソクができたらでいいと思う。
    /// DBから消す方法は？ → 登録と同じでいいと思う。
    /// 
    /// 今の状態だとメモリにはローソクリストを持ってない。
    /// テクニカル計算をするなら持たせるようにすべき。
    /// 
    /// 使い方：MakeGenerationでインスタンスを取得する
    /// </summary>
    public class CandleMaker
    {
        #region Fields
        /// <summary>
        /// ログ
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// データベース
        /// </summary>
        private ApplicationDbContext DbContext { get; }

        /// <summary>
        /// 子
        /// </summary>
        public List<CandleMaker> Children { get; }

        /// <summary>
        /// 最新のTicker
        /// 値を持つのは一番親のみ
        /// 未取得の場合もnull
        /// </summary>
        public Ticker CurrentTicker { get; private set; }

        /// <summary>
        /// 最新のローソク（作成途中のローソク）
        /// 完成したローソクはDBへ
        /// </summary>
        public Candle CurrentCandle { get; private set; }

        /// <summary>
        /// 今まで作ったローソク
        /// </summary>
        public List<Candle> CandleList { get; }

        /// <summary>
        /// 時間足
        /// </summary>
        public MTimeScale TimeScale { get; }

        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// どの板か
        /// </summary>
        public MBoard Board { get; }

        #endregion

        #region コンストラクタ（外から呼ばない）
        /// <summary>
        /// 子を作成するコンストラクタ
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="timeScale"></param>
        /// <param name="displayName"></param>
        /// <param name="board">MBoard</param>
        CandleMaker(ApplicationDbContext dbContext, MTimeScale timeScale, string displayName, MBoard board)
        {
            Children = new List<CandleMaker>();
            DbContext = dbContext;
            CandleList = new List<Candle>();
            TimeScale = timeScale;
            DisplayName = displayName;
            Board = board;
        }
        #endregion

        #region MakeGeneration:親子関係を持ったインスタンス作成
        /// <summary>
        /// 親子関係を付けてCandleMakerインスタンスを作成する
        /// 親に約数が無い秒数の時間足は捨てられる
        /// 外からはコンストラクタではなく、このメソッドで作成する
        /// </summary>
        /// <param name="dbContext">DB接続</param>
        /// <param name="timeScales">時間足リスト、時間が短い順</param>
        /// <param name="board">MBoard</param>
        /// <returns>親子関係付きCandleMakerインスタンス</returns>
        public static CandleMaker MakeGeneration(ILogger logger, ApplicationDbContext dbContext, List<MTimeScale> timeScales, MBoard board)
        {
            // 元のリストに影響を与えないよう、コピーして使用する
            var execList = new List<MTimeScale>(timeScales);

            // 一番親の要素
            CandleMaker result = null;

            // まだ親が見つかっていない子要素はここに保持する
            List<CandleMaker> children = new List<CandleMaker>();

            // 要素が無くなるまで繰り返す
            while(execList.Count > 0)
            {
                // 最後から1つ取り出す
                var longest = execList.Last();
                execList.Remove(longest);

                // インスタンス作成し、一旦親に設定する
                result = new CandleMaker(dbContext, longest, longest.DisplayName, board);

                // 取り出したものが親か確認する
                List<CandleMaker> delList = new List<CandleMaker>();
                foreach (var item in children)
                {
                    if(item.TimeScale.SecondsValue % result.TimeScale.SecondsValue == 0)
                    {
                        // 約数ならば親に設定、子ども候補から削除
                        result.Children.Add(item);
                        delList.Add(item);
                    }
                }
                foreach (var item in delList)
                {
                    children.Remove(item);
                }

                // 子ども候補にも入れる
                children.Add(result);
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 親専用
        /// Tickerでローソクを更新する
        /// </summary>
        /// <param name="ticker">Ticker</param>
        public void Update(Ticker ticker)
        {
            // 現在のTickerに設定
            CurrentTicker = ticker;

            if (CurrentCandle == null)
            {
                // 起動してから初回の動作
                if (SystemConstants.IsFirstDeleteCandle)
                {
                    // この板の全ローソクデータを削除
                    var delList = DbContext.Candles.Where(d => d.Board.Id == Board.Id).ToList();
                    DbContext.Candles.RemoveRange(delList);
                    DbContext.SaveChanges();
                }

                // 新しいローソクの準備
                CurrentCandle = new Candle(Board, TimeScale, CurrentTicker);
            }
            else // 初回ではない場合
            {
                // ローソクを更新
                var newCandles = CurrentCandle.UpdateByTicker(CurrentTicker);

                foreach (var item in newCandles)
                {
                    // 新しいローソクがある場合
                    // 現在のローソクでDB更新
                    DbContext.Candles.Add(CurrentCandle);

                    // 子要素を新しいローソクで更新
                    UpdateChildren(item);

                    // 新しいローソクをセット
                    CurrentCandle = item;
                }
                if (newCandles.Count > 0)
                {
                    // 更新があった場合
                    // 最大データ数を超えていたら、古いデータを1件削除
                    DeleteOldData();

                    // 子要素も更新が終わったらコミット
                    DbContext.SaveChanges(SystemConstants.SystemName);
                }
            }
        }

        /// <summary>
        /// 親子共通
        /// 最大データ数を超えていたら古いデータを削除する
        /// 1件だけ
        /// </summary>
        private void DeleteOldData()
        {
            var count = DbContext.Candles.Where(d => d.TimeScale.Id == TimeScale.Id && d.Board.Id == Board.Id).Count();
            int delCount = count - SystemConstants.MaxCandle;
            if (delCount > 0)
            {
                // 古いデータを削除
                // ※遅いようだったらOrderByの使用をやめておく
                DbContext.Candles.Remove(
                    DbContext.Candles.OrderBy(d => d.Id).First(d => d.TimeScale.Id == TimeScale.Id && d.Board.Id == Board.Id)
                    );
            }
        }

        /// <summary>
        /// 子を更新する
        /// </summary>
        /// <param name="candle"></param>
        public void UpdateChildren(Candle candle)
        {
            foreach (var item in Children)
            {
                item.UpdateByCandle(candle);
            }
        }

        /// <summary>
        /// 子専用
        /// 親から送られてきたローソクで更新する
        /// </summary>
        /// <param name="candle">ローソクデータ</param>
        public void UpdateByCandle(Candle candle)
        {
            if (CurrentCandle == null)
            {
                // 初回
                // 新しいローソクの準備
                CurrentCandle = new Candle(Board, TimeScale, candle);

                // 子要素を更新
                UpdateChildren(candle);
            }
            else
            {
                // 2回目以降
                var newCandles = CurrentCandle.UpdateByCandle(candle);

                foreach (var item in newCandles)
                {
                    // 新しいローソクがある場合
                    // 現在のローソクでDB更新
                    DbContext.Candles.Add(CurrentCandle);

                    // 子要素を新しいローソクで更新
                    UpdateChildren(item);

                    // 新しいローソクをセット
                    CurrentCandle = item;
                }
                if (newCandles.Count > 0)
                {
                    // 更新があった場合
                    // 最大データ数を超えていたら、古いデータを1件削除
                    DeleteOldData();
                }
            }
        }
    }
}
