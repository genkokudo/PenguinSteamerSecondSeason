using EfCore.Shaman;
using PenguinSteamerSecondSeason.Data;
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
        // TODO:複数分飛んだ時の複数本作成が考慮されていない

        #region Fields
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
        /// ローソク開始時刻
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// ローソク終了時刻
        /// </summary>
        public DateTime EndTime { get; set; }

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
        #endregion

        #region Initialize
        /// <summary>
        /// ローソク足データ
        /// （このコンストラクタはデータ読み込み以外で使わない）
        /// </summary>
        public Candle()
        {
        }

        /// <summary>
        /// ローソク足データ
        /// 親ローソクメーカー用
        /// タイムスタンプは時間足の単位で切り捨てるが、1日以上は切り捨てないことに注意。
        /// </summary>
        /// <param name="board">板</param>
        /// <param name="timeScale">時間足</param>
        /// <param name="ticker">現在のTicker</param>
        public Candle(MBoard board, MTimeScale timeScale, Ticker ticker)
        {
            Board = board;
            TimeScale = timeScale;

            // 切り捨てて、ローソクの開始時刻を求める
            BeginTime = ticker.Timestamp;
            int TimeSeconds = ((BeginTime.Hour * 60 + BeginTime.Minute) * 60 + BeginTime.Second);
            BeginTime = BeginTime.AddSeconds(-(TimeSeconds % TimeScale.SecondsValue));

            // 終了時刻を求める
            EndTime = BeginTime.AddSeconds(TimeScale.SecondsValue);

            Min = UNKNOWN;
            Max = UNKNOWN;
            Begin = UNKNOWN;
            End = UNKNOWN;
            Volume = UNKNOWN;

            // Tickerによる更新を行う
            UpdateByTicker(ticker);
        }

        /// <summary>
        /// ローソク足データ
        /// 子ローソクメーカー用
        /// タイムスタンプは時間足の単位で切り捨てるが、1日以上は切り捨てないことに注意。
        /// </summary>
        /// <param name="board">板</param>
        /// <param name="timeScale">時間足</param>
        /// <param name="candle">親から送られてきたローソク</param>
        public Candle(MBoard board, MTimeScale timeScale, Candle candle)
        {
            Board = board;
            TimeScale = timeScale;

            // 開始時刻
            BeginTime = candle.BeginTime;

            // 終了時刻を求める
            EndTime = BeginTime.AddSeconds(TimeScale.SecondsValue);

            Min = candle.Min;
            Max = candle.Max;
            Begin = candle.Begin;
            End = candle.End;
            Volume = candle.Volume;

            // Candleによる更新を行う
            UpdateByCandle(candle);
        }
        #endregion

        /// <summary>
        /// Tickerによってローソクを更新する
        /// 使用するときは戻り値をnullチェックすること
        /// </summary>
        /// <param name="ticker">Ticker</param>
        /// <returns>終了していれば新しいローソクを返す、そうでなければnull</returns>
        public Candle UpdateByTicker(Ticker ticker)
        {
            // 終了判定を行い、終了時間が過ぎていれば新しいローソクを作成して返す
            if(ticker.Timestamp > EndTime)
            {
                return new Candle(Board, TimeScale, ticker);
            }

            // 終値
            End = ticker.Ltp;
            
            // 始値（初回のみ）
            if (Begin < 0)
            {
                Begin = ticker.Ltp;
            }

            // 最小値
            if (Min < 0)
            {
                // 初回
                Min = ticker.Ltp;
            }
            else
            {
                Min = Math.Min(Min, ticker.Ltp);
            }

            // 最大値
            Max = Math.Max(Max, ticker.Ltp);

            // ボリューム
            Volume = ticker.Volume;

            return null;
        }

        /// <summary>
        /// ローソクデータによってローソクを更新する
        /// 使用するときは戻り値をnullチェックすること
        /// </summary>
        /// <param name="candle">親から送られてきたローソク</param>
        /// <returns>終了していれば新しいローソクを返す、そうでなければnull</returns>
        public Candle UpdateByCandle(Candle candle)
        {
            // 終了判定を行い、終了時間が過ぎていれば新しいローソクを作成して返す
            if (candle.BeginTime > EndTime)
            {
                return new Candle(Board, TimeScale, candle);
            }

            // 終値
            End = candle.End;

            // 始値（初回のみ）
            if (Begin < 0)
            {
                Begin = candle.Begin;
            }

            // 最小値
            if (Min < 0)
            {
                // 初回
                Min = candle.Min;
            }
            else
            {
                Min = Math.Min(Min, candle.Min);
            }

            // 最大値
            Max = Math.Max(Max, candle.Max);

            // ボリューム
            Volume = candle.Volume;

            return null;
        }

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
