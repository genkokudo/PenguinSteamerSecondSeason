// 没

//using Microsoft.Extensions.Logging;
//using PenguinSteamerSecondSeason.Data;
//using PenguinSteamerSecondSeason.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace PenguinSteamerSecondSeason.Common
//{
//    /// <summary>
//    /// 最新のTickerの値を保持する？→管理クラスで良くない？
//    /// 取引頻度が低い銘柄の為に最低何件か計算用を残す。→ローソク作成クラスの役目（ローソクでも登録間隔は検討）
//    /// DBから消す方法は？
//    /// </summary>
//    class MyTickerUpdate
//    {
//        /// <summary>
//        /// 最小登録間隔（分）
//        /// 最小分足
//        /// </summary>
//        public int SubmitMinutes { get; set; }

//        /// <summary>
//        /// 計算用Tickerの最大件数
//        /// </summary>
//        public int MaxTimeScaleCount { get; set; }

//        /// <summary>
//        /// ログ
//        /// </summary>
//        ILogger Logger { get; }

//        /// <summary>
//        /// 溜めているデータ
//        /// 保存用
//        /// ローソクで保存するし、いらないのでは？
//        /// </summary>
//        List<Ticker> DataForSave { get; }

//        /// <summary>
//        /// 溜めているデータ
//        /// 計算用
//        /// </summary>
//        List<Ticker> DataForCalclation { get; }

//        /// <summary>
//        /// 1つのTickerに関して、計算用にデータを溜める
//        /// 一定時間ごとにDBに保存する
//        /// </summary>
//        /// <param name="logger"></param>
//        /// <param name="submitMinutes"></param>
//        public MyTickerUpdate(ILogger logger, int submitMinutes)
//        {
//            Logger = logger;
//            SubmitMinutes = submitMinutes;
//            DataForSave = new List<Ticker>();
//            DataForCalclation = new List<Ticker>();
//            MaxTimeScaleCount = 5;  // そんなにいらないと思う
//        }

//        /// <summary>
//        /// データを追加する
//        /// </summary>
//        /// <param name="ticker">データ</param>
//        public Task AddAsync(Ticker ticker)
//        {
//            return Task.Run(() =>
//            {
//                DataForSave.Add(ticker);
//                DataForCalclation.Add(ticker);

//                // 計算用データが上限超えたら早いのから削除
//            });
//        }

//        /// <summary>
//        /// 現在保存用に溜まっているデータを保存する
//        /// これローソクに移動した方が良い
//        /// </summary>
//        /// <returns></returns>
//        private async Task SubmitDataAsync()
//        {
//            while (true)
//            {
//                Logger.LogInformation($"Tickerデータ登録:");
//                foreach (var item in DataForSave)
//                {
//                    // ローソクに集計
//                    // 集計したものを削除
//                }
//                await Task.Delay(2000);
//                //await Task.Delay(SubmitMinutes * 1000 * 60);
//            }
//        }

//        /// <summary>
//        /// 計算用データを削除する
//        /// </summary>
//        /// <returns></returns>
//        private async Task RemoveDataAsync()
//        {
//            while (true)
//            {
//                Logger.LogInformation($"Tickerデータ削除:");
//                await Task.Delay(2000);
//                //await Task.Delay(SubmitMinutes * 1000 * 60);
//            }
//        }
//    }
//}
