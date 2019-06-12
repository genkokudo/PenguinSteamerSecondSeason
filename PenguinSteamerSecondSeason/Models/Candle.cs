using EfCore.Shaman;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PenguinSteamerSecondSeason.Models
{
    #region Candle:ローソクデータ
    /// <summary>
    /// ローソクデータ
    /// </summary>
    public class Candle : IEntity
    {
        /// <summary>
        /// 初期値
        /// </summary>
        public const int UNKNOWN = -1;

        /// <summary>
        /// 通し番号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// どの取引所、通貨ペアか
        /// </summary>
        [Index("BoardAndTimeScale", 1)]    // 複合インデックス
        public MBoard Board { get; set; }

        /// <summary>
        /// 時間足
        /// </summary>
        [Index("BoardAndTimeScale", 2)]    // 複合インデックス
        public MTimeScale TimeScale { get; set; }

        /// <summary>
        /// 時刻
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 最小値
        /// </summary>
        public decimal Min { get; set; }

        /// <summary>
        /// 最大値
        /// </summary>
        public decimal Max { get; set; }

        /// <summary>
        /// 始値
        /// </summary>
        public decimal Begin { get; set; }

        /// <summary>
        /// 終値
        /// </summary>
        public decimal End { get; set; }

        /// <summary>
        /// ボリューム
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// ローソク足データ
        /// </summary>
        public Candle()
        {
            TimeStamp = new DateTime();
            Min = UNKNOWN;
            Max = UNKNOWN;
            Begin = UNKNOWN;
            End = UNKNOWN;
            Volume = UNKNOWN;
        }

        /// <summary>
        /// ローソク足データ
        /// </summary>
        public Candle(DateTime dateTime)
        {
            TimeStamp = dateTime;
            Min = UNKNOWN;
            Max = UNKNOWN;
            Begin = UNKNOWN;
            End = UNKNOWN;
            Volume = UNKNOWN;
        }

        /// <summary>
        /// 1分足の最大と最小を
        /// リセットする
        /// 各値を現在の終値でセット
        /// </summary>
        public void ResetMinMax()
        {
            Begin = End;
            Min = End;
            Max = End;
        }

        ///// <summary>
        ///// Tickerによってろうそくを更新する
        ///// </summary>
        ///// <param name="ticker">Ticker</param>
        //public void UpdateByTicker(Ticker ticker)
        //{
        //    // 終値
        //    End = ticker.Itp;
        //    // 始値（初回のみ）
        //    if (Begin < 0)
        //    {
        //        Begin = ticker.Itp;
        //    }
        //    if (Min < 0)
        //    {
        //        Min = ticker.Itp;
        //    }
        //    else
        //    {
        //        Min = Math.Min(Min, ticker.Itp);
        //    }
        //    Max = Math.Max(Max, ticker.Itp);
        //}

        #region 共通項目
        /// <summary>
        /// 登録者
        /// </summary>
        [StringLength(255)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [StringLength(255)]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// 論理削除フラグ
        /// </summary>
        public bool IsDeleted { get; set; }
        #endregion
    }
    #endregion
}
