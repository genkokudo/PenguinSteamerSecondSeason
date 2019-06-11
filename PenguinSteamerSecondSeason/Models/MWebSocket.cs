using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PenguinSteamerSecondSeason.Models
{
    /// <summary>
    /// WebSocket設定
    /// </summary>
    public class MWebSocket : IEntity
    {
        /// <summary>
        /// 通し番号
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// どの取引所のどの通貨ペアか
        /// </summary>
        public MBoard Board { get; set; }
        
        /// <summary>
        /// 種類：1:Ticker、2:板、3:約定…
        /// </summary>
        public int Category { get; set; }
        
        /// <summary>
        /// エンドポイント
        /// </summary>
        public string EndPoint { get; set; }
        
        /// <summary>
        /// チャンネル名
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 受信するかどうか
        /// </summary>
        public bool IsEnabled { get; set; }


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
