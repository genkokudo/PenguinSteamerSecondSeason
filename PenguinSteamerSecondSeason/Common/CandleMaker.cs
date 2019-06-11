using System;
using System.Collections.Generic;
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
    /// 登録間隔は検討する
    /// DBから消す方法は？
    /// </summary>
    class CandleMaker
    {

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

        ///// <summary>
        ///// 溜めているデータ
        ///// 保存用
        ///// ローソクで保存するし、いらないのでは？
        ///// </summary>
        //List<Ticker> DataForSave { get; }

        ///// <summary>
        ///// 溜めているデータ
        ///// 計算用
        ///// </summary>
        //List<Ticker> DataForCalclation { get; }
    }
}
