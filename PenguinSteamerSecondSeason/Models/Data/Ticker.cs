using PenguinSteamerSecondSeason.Models.Enums;
using PenguinSteamerSecondSeason.Models.RawData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PenguinSteamerSecondSeason.Data
{
    /// <summary>
    /// Ticker
    /// </summary>
    public class Ticker
    {
        public Ticker()
        {
            // 何もしない
        }

        /// <summary>
        /// Bitflyerからの変換
        /// </summary>
        /// <param name="lightningTicker"></param>
        public Ticker(LightningTicker lightningTicker)
        {
            // 世界標準時なので9時間遅い、直すかどうかは他の取引所と合わせること
            // "timestamp": "2019-06-23T16:10:41.0272447Z"
            Timestamp = lightningTicker.timestamp;
            TickId = lightningTicker.tick_id;
            BestBid = (decimal)lightningTicker.best_bid;
            BestAsk = (decimal)lightningTicker.best_ask;
            BestBidSize = (decimal)lightningTicker.best_bid_size;
            BestAskSize = (decimal)lightningTicker.best_ask_size;
            TotalBidDepth = (decimal)lightningTicker.total_bid_depth;
            TotalAskDepth = (decimal)lightningTicker.total_ask_depth;
            Ltp = (decimal)lightningTicker.ltp;
            Volume = (decimal)lightningTicker.volume;
            VolumeByProduct = (decimal)lightningTicker.volume_by_product;
        }

        /// <summary>
        /// 通し番号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// "2019-06-08T04:52:56.5062738Z"
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// 96100459
        /// 他の取引所によっては、Stringになるかも
        /// </summary>
        public int TickId { get; set; }
        /// <summary>
        /// 買い注文
        /// 893201.0
        /// </summary>
        public decimal BestBid { get; set; }
        /// <summary>
        /// 売り注文
        /// 893315.0
        /// </summary>
        public decimal BestAsk { get; set; }
        /// <summary>
        /// 0.1
        /// </summary>
        public decimal BestBidSize { get; set; }
        /// <summary>
        /// 0.05
        /// </summary>
        public decimal BestAskSize { get; set; }
        /// <summary>
        /// 買い注文の量合計
        /// 5834.23943722
        /// </summary>
        public decimal TotalBidDepth { get; set; }
        /// <summary>
        /// 売り注文の量合計
        /// 5306.34163044
        /// </summary>
        public decimal TotalAskDepth { get; set; }
        /// <summary>
        /// 最終価格
        /// </summary>
        public decimal Ltp { get; set; }
        /// <summary>
        /// 24時間の出来高
        /// 120150.50692233
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// 24時間の出来高（この通貨ペアのみ）
        /// 120150.50692233
        /// </summary>
        public decimal VolumeByProduct { get; set; }
    }
}
