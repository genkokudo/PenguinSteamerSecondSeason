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
    /// 取引頻度が低い銘柄の為に最低何件か計算用を残す？
    /// 登録間隔は検討する → 取り敢えず随時でいいと思う。
    /// DBから消す方法は？ → 登録と同じでいいと思う。
    /// </summary>
    public class CandleMaker
    {
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
        /// 最新のローソク（作成途中）
        /// 完成したローソクはDBへ
        /// </summary>
        public Candle CurrentCandle { get; private set; }

        /// <summary>
        /// 今まで作ったローソク
        /// </summary>
        public List<Candle> CandleList { get; }

        /// <summary>
        /// 何秒の足か
        /// </summary>
        public int Seconds { get; }

        /// <summary>
        /// 現在、前回のローソク作成完了から何秒経過したか
        /// </summary>
        public int CurrentSeconds { get; private set; }

        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// 外からは呼ばないこと
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="seconds"></param>
        /// <param name="displayName"></param>
        public CandleMaker(ApplicationDbContext dbContext, int seconds, string displayName)
        {
            Children = new List<CandleMaker>();
            DbContext = dbContext;
            CandleList = new List<Candle>();
            Seconds = seconds;
            DisplayName = displayName;

            // TODO:最初のローソクを作成時、タイムスタンプを設定
            // TODO:初回更新時に0時0分からの経過時間を秒数で割った余りを代入
            // 初回は一応ローソクデータ全消ししよう（設定可）
            CurrentSeconds = -1;
        }

        /// <summary>
        /// 親子関係を付けてCandleMakerインスタンスを作成する
        /// 親に約数が無い秒数の時間足は捨てられる
        /// 外からはコンストラクタではなく、このメソッドで作成する
        /// </summary>
        /// <param name="dbContext">DB接続</param>
        /// <param name="timeScales">時間足リスト、時間が短い順</param>
        /// <returns>親子関係付きCandleMakerインスタンス</returns>
        public static CandleMaker MakeGeneration(ApplicationDbContext dbContext, List<MTimeScale> timeScales)
        {
            // 一番親の要素
            CandleMaker result = null;

            // まだ親が見つかっていない子要素はここに保持する
            List<CandleMaker> children = new List<CandleMaker>();

            // 要素が無くなるまで繰り返す
            while(timeScales.Count > 0)
            {
                // 最後から1つ取り出す
                var longest = timeScales.Last();
                timeScales.Remove(longest);

                // インスタンス作成し、一旦親に設定する
                result = new CandleMaker(dbContext, longest.SecondsValue, longest.DisplayName);

                // 取り出したものが親か確認する
                List<CandleMaker> delList = new List<CandleMaker>();
                foreach (var item in children)
                {
                    if(item.Seconds % result.Seconds == 0)
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

        /// <summary>
        /// 親専用
        /// Tickerでローソクを更新する
        /// </summary>
        /// <param name="ticker"></param>
        public void Update(Ticker ticker)
        {

            //SystemConstants.MaxCandle;
            if (true)   // 1分経過していたら更新、複数分更新していたらその数だけローソクを作成
            {
                //UpdateChildren();
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
                item.UpdateByCandle(candle, Seconds);
            }
        }

        /// <summary>
        /// 子専用
        /// 親から送られてきたローソクで更新する
        /// </summary>
        /// <param name="candle">ローソクデータ</param>
        /// <param name="seconds">親が何秒足か</param>
        public void UpdateByCandle(Candle candle, int seconds)
        {

        }


        ///// <summary>
        ///// 現在保存用に溜まっているデータを保存する
        ///// これローソクに移動した方が良い
        ///// </summary>
        ///// <returns></returns>
        //private async Task SubmitDataAsync()
        //{
        //    while (true)
        //    {
        //        Logger.LogInformation($"Tickerデータ登録:");
        //        foreach (var item in DataForSave)
        //        {
        //            // ローソクに集計
        //            // 集計したものを削除
        //        }
        //        await Task.Delay(2000);
        //        //await Task.Delay(SubmitMinutes * 1000 * 60);
        //    }
        //}

        ///// <summary>
        ///// 1つのTickerに関して、計算用にデータを溜める
        ///// 一定時間ごとにDBに保存する
        ///// </summary>
        ///// <param name="logger"></param>
        ///// <param name="submitMinutes"></param>
        //public CandleMaker(ILogger logger, int submitMinutes)
        //{
        //    Logger = logger;
        //    SubmitMinutes = submitMinutes;
        //    DataForSave = new List<Ticker>();
        //    DataForCalclation = new List<Ticker>();
        //    MaxTimeScaleCount = 5;  // そんなにいらないと思う
        //}
    }
}
