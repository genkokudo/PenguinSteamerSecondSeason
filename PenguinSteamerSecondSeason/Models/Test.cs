using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PenguinSteamerSecondSeason.Models
{
    class Test : IEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        [Required, Column(TypeName = "text"), StringLength(50), Display(Name = "タイトル")]
        public string Title { get; set; }

        /// <summary>
        /// 問題
        /// </summary>
        [Required, Column(TypeName = "text"), Display(Name = "問題")]
        public string Question { get; set; }

        /// <summary>
        /// 回答
        /// </summary>
        [Column(TypeName = "text"), Display(Name = "回答")]
        public string Answer { get; set; }

        #region 共通項目
        /// <summary>
        /// 登録者
        /// </summary>
        [Index, DisplayName("登録者"), StringLength(255)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [DisplayName("登録日時")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [DisplayName("更新者"), StringLength(255)]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [DisplayName("更新日時")]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// 論理削除フラグ
        /// </summary>
        [DisplayName("論理削除")]
        public bool IsDeleted { get; set; }
        #endregion
    }
}
