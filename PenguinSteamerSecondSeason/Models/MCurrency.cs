using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PenguinSteamerSecondSeason.Models
{
    /// <summary>
    /// 通貨マスタ
    /// コイン名を管理する
    /// </summary>
    public class MCurrency : IEntity
    {
        /// <summary>
        /// 通し番号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { get; set; }

        // 為替を扱う場合、ドル換算係数とか、スワップとかが必要？スワップは板マスタかなあ。

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
