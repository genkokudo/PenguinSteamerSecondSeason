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
        #region Fields

        /// <summary>
        /// 通し番号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// どの取引所、通貨ペアか
        /// </summary>
        [Index("BoardAndTimeScale", 1)]    // 複合インデックス
        public MBoard MBoard { get; set; }

        /// <summary>
        /// 時間足
        /// </summary>
        [Index("BoardAndTimeScale", 2)]    // 複合インデックス
        public MTimeScale MTimeScale { get; set; }

        /// <summary>
        /// ローソク開始時刻
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// ローソク終了時刻
        /// 注意：この時刻は現在のローソクに含まない
        /// 例えばこれが"08/18/2018 07:23:00"の場合、
        /// 07:22:59はこのローソクの範囲内だが、07:23:00は次のローソクの範囲である。
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
        /// 親ローソクメーカー新規用
        /// タイムスタンプは時間足の単位で切り捨てるが、1日以上は切り捨てないことに注意。
        /// </summary>
        /// <param name="board">板</param>
        /// <param name="timeScale">時間足</param>
        /// <param name="ticker">現在のTicker</param>
        public Candle(MBoard board, MTimeScale timeScale, Ticker ticker)
        {
            MBoard = board;
            MTimeScale = timeScale;

            // 切り捨てて、ローソクの開始時刻を求める
            BeginTime = ticker.Timestamp;
            int TimeSeconds = ((BeginTime.Hour * 60 + BeginTime.Minute) * 60 + BeginTime.Second);
            BeginTime = BeginTime.AddSeconds(-(TimeSeconds % MTimeScale.SecondsValue));
            BeginTime.AddMilliseconds(-BeginTime.Millisecond);  // ミリ秒切り捨て

            // 終了時刻を求める
            EndTime = BeginTime.AddSeconds(MTimeScale.SecondsValue);

            Min = ticker.Ltp;
            Max = ticker.Ltp;
            Begin = ticker.Ltp;
            End = ticker.Ltp;
            Volume = ticker.Volume;
        }

        /// <summary>
        /// ローソク足データ
        /// 子ローソクメーカー新規用
        /// タイムスタンプは時間足の単位で切り捨てるが、1日以上は切り捨てないことに注意。
        /// </summary>
        /// <param name="board">板</param>
        /// <param name="timeScale">時間足</param>
        /// <param name="candle">親から送られてきたローソク</param>
        public Candle(MBoard board, MTimeScale timeScale, Candle candle)
        {
            MBoard = board;
            MTimeScale = timeScale;

            // 開始時刻は、この時間足で切り捨てる
            BeginTime = candle.BeginTime;
            int TimeSeconds = ((BeginTime.Hour * 60 + BeginTime.Minute) * 60 + BeginTime.Second);
            BeginTime = BeginTime.AddSeconds(-(TimeSeconds % MTimeScale.SecondsValue));
            BeginTime.AddMilliseconds(-BeginTime.Millisecond);  // ミリ秒切り捨て

            // 終了時刻を求める
            EndTime = BeginTime.AddSeconds(MTimeScale.SecondsValue);

            Min = candle.End;
            Max = candle.End;
            Begin = candle.End;
            End = candle.End;
            Volume = candle.Volume;

            //Min = candle.Min;
            //Max = candle.Max;
            //Begin = candle.Begin;
            //End = candle.End;
            //Volume = candle.Volume;
        }

        /// <summary>
        /// ローソク足データ
        /// 親ローソクメーカー更新時の作成
        /// 子ローソクメーカー更新時の作成用
        /// </summary>
        /// <param name="board">板</param>
        /// <param name="timeScale">時間足</param>
        /// <param name="candle">親から送られてきたローソク</param>
        /// <param name="endTime">新しく作成するローソクの終了時刻</param>
        private Candle(MBoard board, MTimeScale timeScale, Candle candle, DateTime endTime)
        {
            MBoard = board;
            MTimeScale = timeScale;
            EndTime = endTime;
            BeginTime = EndTime.AddSeconds(-MTimeScale.SecondsValue);

            Min = candle.End;
            Max = candle.End;
            Begin = candle.End;
            End = candle.End;
            Volume = candle.Volume;
        }
        #endregion

        /// <summary>
        /// Tickerによってローソクを更新する
        /// 終了している場合は更新されない
        /// </summary>
        /// <param name="ticker">Ticker</param>
        /// <returns>終了している場合は新しいローソクが返される。時間が空いた場合は複数返すこともある</returns>
        public List<Candle> UpdateByTicker(Ticker ticker)
        {
            var result = new List<Candle>();

            // 終了判定を行い、終了時間が過ぎている間繰り返す
            var CurrentEndTime = EndTime;
            while (ticker.Timestamp >= CurrentEndTime)
            {
                // 新しいローソクを作成
                CurrentEndTime = CurrentEndTime.AddSeconds(MTimeScale.SecondsValue);
                var newCandle = new Candle(MBoard, MTimeScale, this, CurrentEndTime);
                if(newCandle.EndTime > ticker.Timestamp)
                {
                    // 新しいローソクの範囲内にTickerがあるときは、tickerの値をローソクに反映
                    newCandle.Begin = ticker.Ltp;
                    newCandle.End = ticker.Ltp;
                    newCandle.Min = ticker.Ltp;
                    newCandle.Max = ticker.Ltp;
                    newCandle.Volume = ticker.Volume;
                }
                result.Add(newCandle);
            }
            if (result.Count == 0)
            {
                // 終了していない場合は更新
                // 終値
                End = ticker.Ltp;

                // 最小値
                Min = Math.Min(Min, ticker.Ltp);

                // 最大値
                Max = Math.Max(Max, ticker.Ltp);

                // ボリューム
                Volume = ticker.Volume;
            }

            return result;
        }

        /// <summary>
        /// ローソクデータによってローソクを更新する
        /// 終了している場合は更新されない
        /// </summary>
        /// <param name="candle">親から送られてきたローソク</param>
        /// <returns>終了している場合は新しいローソクが返される。時間が空いた場合は複数返すこともある</returns>
        public List<Candle> UpdateByCandle(Candle candle)
        {
            var result = new List<Candle>();

            // 終了判定を行い、終了時間が過ぎている間繰り返す
            var CurrentEndTime = EndTime;
            while (candle.BeginTime >= CurrentEndTime)
            {
                CurrentEndTime = CurrentEndTime.AddSeconds(MTimeScale.SecondsValue);
                var newCandle = new Candle(MBoard, MTimeScale, this, CurrentEndTime);
                if (newCandle.EndTime > candle.BeginTime)
                {
                    // candleの開始時間が新しいローソクの範囲内ならば、最新の値（終値）をローソクに反映
                    newCandle.Begin = candle.End;
                    newCandle.End = candle.End;
                    newCandle.Min = candle.End;
                    newCandle.Max = candle.End;
                    newCandle.Volume = candle.Volume;
                }
                result.Add(newCandle);
            }
            if (result.Count == 0)
            {
                // 終了していない場合は更新

                // 終値
                End = candle.End;

                // 最小値
                Min = Math.Min(Min, candle.Min);

                // 最大値
                Max = Math.Max(Max, candle.Max);

                // ボリューム
                Volume = candle.Volume;
            }

            return result;
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
