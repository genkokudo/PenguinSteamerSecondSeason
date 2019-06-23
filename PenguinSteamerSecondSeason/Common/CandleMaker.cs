using Microsoft.EntityFrameworkCore;
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
        /// どの板か
        /// </summary>
        public MBoard Board { get; }

        #endregion

        #region コンストラクタ（外から呼ばない）
        /// <summary>
        /// 親または子を作成するコンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="dbContext">DB接続</param>
        /// <param name="timeScales">時間足リスト、時間が短い順</param>
        /// <param name="board">板情報</param>
        CandleMaker(ILogger logger, ApplicationDbContext dbContext, MTimeScale timeScale, MBoard board)
        {
            logger.LogInformation($"CandleMaker親子作成:{board.Name} {timeScale.DisplayName}");
            Logger = logger;
            Children = new List<CandleMaker>();
            DbContext = dbContext;
            CandleList = new List<Candle>();
            TimeScale = timeScale;
            Board = board;
        }
        #endregion

        #region MakeGeneration:親子関係を持ったインスタンス作成
        /// <summary>
        /// 親子関係を付けてCandleMakerインスタンスを作成する
        /// 親に約数が無い秒数の時間足は捨てられる
        /// 外からはコンストラクタではなく、このメソッドで作成する
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="dbContext">DB接続</param>
        /// <param name="timeScales">時間足リスト、時間が短い順</param>
        /// <param name="board">MBoard</param>
        /// <returns>親子関係付きCandleMakerインスタンス</returns>
        public static CandleMaker MakeGeneration(ILogger logger, ApplicationDbContext dbContext, List<MTimeScale> timeScales, MBoard board)
        {
            logger.LogInformation($"CandleMaker作成:{board.Name}");

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
                result = new CandleMaker(logger, dbContext, longest, board);

                // 取り出したものが親か確認する
                List<CandleMaker> delList = new List<CandleMaker>();
                foreach (var item in children)
                {
                    if(item.TimeScale.SecondsValue % result.TimeScale.SecondsValue == 0)
                    {
                        // 約数ならば親に設定、子ども候補から削除
                        logger.LogDebug($"親子関係設定:{result.TimeScale.DisplayName} <- {item.TimeScale.DisplayName}");
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

        #region Update
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
                    Logger.LogInformation($"ローソクデータ全削除:{Board.Name}");
                    var delList = DbContext.Candles.Include(x => x.MBoard).Where(d => d.MBoard.MBoardId == Board.MBoardId).ToList();
                    DbContext.Candles.RemoveRange(delList);
                    DbContext.SaveChanges();
                }

                // 新しいローソクの準備
                CurrentCandle = new Candle(Board, TimeScale, CurrentTicker);

                // 子要素を新しいローソクで更新
                UpdateChildren(CurrentCandle);
            }
            else // 初回ではない場合
            {
                // ローソクを更新
                var newCandles = CurrentCandle.UpdateByTicker(CurrentTicker);

                foreach (var item in newCandles)
                {
                    // 新しいローソクがある場合
                    // 現在のローソクでDB更新
                    Logger.LogDebug($"DB:Candles更新:{Board.Name} {TimeScale.DisplayName}");
                    DbContext.Candles.Add(CurrentCandle);

                    // 今まで作成したローソクに追加
                    CandleList.Add(CurrentCandle);

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
            // コミットしていない分は数えられないことに注意
            // 今は起動のたびに消しているから良いが、メモリ内ローソクとDBのローソクの数が異なる場合があることも注意
            // →コミット済みに関して、メモリローソクの数を上回った分を削除するようにする
            if(CandleList.Count > SystemConstants.MaxCandle)
            {
                // 今まで作成したローソクから削除
                int delCount = CandleList.Count - SystemConstants.MaxCandle;
                for (int i = 0; i < delCount; i++)
                {
                    CandleList.RemoveAt(0);
                }
            }

            var count = DbContext.Candles.Include(x => x.MTimeScale).Include(x => x.MBoard).Where(d => d.MTimeScale.Id == TimeScale.Id && d.MBoard.MBoardId == Board.MBoardId).Count();
            // DBの、メモリのローソクよりも多い分を削除（今回コミットされてないものは除くので、1件ズレたりする）
            // ※遅いようだったらOrderByの使用をやめておく
            if (count > CandleList.Count)
            {
                int delCount = count - CandleList.Count;
                Logger.LogDebug($"Candle{delCount}件削除:{Board.Name} {TimeScale.DisplayName}");
                var delList = new List<Candle>(delCount);
                var dataList = DbContext.Candles.Where(d => d.MTimeScale.Id == TimeScale.Id && d.MBoard.MBoardId == Board.MBoardId).OrderBy(d => d.Id).ToList();

                for (int i = 0; i < delCount; i++)
                {
                    delList.Add(dataList[i]);
                }
                foreach (var item in delList)
                {
                    DbContext.Candles.Remove(item);
                }
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
            Logger.LogDebug($"DB:Candles更新:{Board.Name} {TimeScale.DisplayName}");
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

                    // 今まで作成したローソクに追加
                    CandleList.Add(CurrentCandle);

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
        #endregion
    }
}
