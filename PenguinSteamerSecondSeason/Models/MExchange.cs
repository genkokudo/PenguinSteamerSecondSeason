//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Text;

//namespace PenguinSteamerSecondSeason.Models
//{
//    /// <summary>
//    /// 取引所マスタ
//    /// 同一の取引所でも現物・FX・先物などに分かれていたら異なる取引所扱いにする
//    /// </summary>
//    public class MExchange : IEntity
//    {
//        /// <summary>
//        /// 通し番号
//        /// IDによって使用するAPIを変える必要があるので自動採番しないようにする
//        /// </summary>
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]   // IDは明示的に指定したいので自動採番OFF
//        [Key]
//        public int Id { get; set; }
//        /// <summary>
//        /// 名前
//        /// </summary>
//        public string Name { get; set; }
//        /// <summary>
//        /// 表示名
//        /// </summary>
//        public string DisplayName { get; set; }

//        #region 共通項目
//        /// <summary>
//        /// 登録者
//        /// </summary>
//        [StringLength(255)]
//        public string CreatedBy { get; set; }

//        /// <summary>
//        /// 登録日時
//        /// </summary>
//        public DateTime? CreatedDate { get; set; }

//        /// <summary>
//        /// 更新者
//        /// </summary>
//        [StringLength(255)]
//        public string UpdatedBy { get; set; }

//        /// <summary>
//        /// 更新日時
//        /// </summary>
//        public DateTime? UpdatedDate { get; set; }

//        /// <summary>
//        /// 論理削除フラグ
//        /// </summary>
//        public bool IsDeleted { get; set; }
//        #endregion
//    }
//}
