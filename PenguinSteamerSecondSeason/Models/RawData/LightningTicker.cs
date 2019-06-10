using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinSteamerSecondSeason.Models.RawData
{
    /// <summary>
    /// BitflyerのTicker
    /// </summary>
    public class LightningTicker
    {
        /// <summary>
        /// "FX_BTC_JPY", "BTC_JPY", etc...
        /// </summary>
        public string product_code { get; set; }
        /// <summary>
        /// "2019-06-08T04:52:56.5062738Z"
        /// </summary>
        public DateTime timestamp { get; set; }
        /// <summary>
        /// 96100459
        /// </summary>
        public int tick_id { get; set; }
        /// <summary>
        /// 買い注文
        /// 893201.0
        /// </summary>
        public float best_bid { get; set; }
        /// <summary>
        /// 売り注文
        /// 893315.0
        /// </summary>
        public float best_ask { get; set; }
        /// <summary>
        /// 0.1
        /// </summary>
        public float best_bid_size { get; set; }
        /// <summary>
        /// 0.05
        /// </summary>
        public float best_ask_size { get; set; }
        /// <summary>
        /// 買い注文の量合計
        /// 5834.23943722
        /// </summary>
        public float total_bid_depth { get; set; }
        /// <summary>
        /// 売り注文の量合計
        /// 5306.34163044
        /// </summary>
        public float total_ask_depth { get; set; }
        /// <summary>
        /// 最終価格
        /// </summary>
        public float ltp { get; set; }
        /// <summary>
        /// 24時間の出来高
        /// 120150.50692233
        /// </summary>
        public float volume { get; set; }
        /// <summary>
        /// 24時間の出来高（この通貨ペアのみ）
        /// 120150.50692233
        /// </summary>
        public float volume_by_product { get; set; }

    }
}
