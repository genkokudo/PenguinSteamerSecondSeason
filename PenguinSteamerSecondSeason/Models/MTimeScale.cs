using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PenguinSteamerSecondSeason.Models
{
    /// <summary>
    /// 時間足
    /// </summary>
    public class MTimeScale : IEntity
    {
        /// <summary>
        /// 通し番号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 秒数
        /// </summary>
        public int SecondsValue { get; set; }

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
