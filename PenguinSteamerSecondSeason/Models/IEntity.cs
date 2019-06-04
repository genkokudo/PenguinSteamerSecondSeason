using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinSteamerSecondSeason.Models
{
    /// <summary>
    /// 登録日時、更新日時の自動設定を行うためのインタフェース
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 登録者
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        string UpdatedBy { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// 論理削除フラグ
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
