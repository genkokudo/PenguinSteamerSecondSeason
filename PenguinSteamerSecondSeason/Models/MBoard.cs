using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PenguinSteamerSecondSeason.Models
{
    /// <summary>
    /// 板マスタ
    /// ここで言う板とは、取引所（取引サービス）、通貨の組み合わせのこと
    /// BF現物のBTC_ETHとか
    /// ZaifAirFXのBTC_JPYみたいな感じ
    /// 同一取引所でも現物とFXは異なる板扱いにする
    /// </summary>
    public class MBoard : IEntity
    {
        /// <summary>
        /// 通し番号
        /// クラス名+Idにしないと紐づかない事に注意
        /// </summary>
        public int MBoardId { get; set; }
        /// <summary>
        /// 通貨1
        /// </summary>
        public MCurrency MCurrency1 { get; set; }
        /// <summary>
        /// 通貨2
        /// </summary>
        public MCurrency MCurrency2 { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { get; set; }

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
}
