using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PenguinSteamerSecondSeason.Common;
using PenguinSteamerSecondSeason.Data;
using PenguinSteamerSecondSeason.Models;
using PenguinSteamerSecondSeason.Models.RawData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason.Services
{
    // 繋いだWebSocketのうち、Tickerだけを集めて辞書で管理することで、外からアクセスできるようにする。
    // 各Tickerの最新の値を持っておく（というか、アクセスできるようにする）
    // メソッドで↑を一覧表示する

    #region Interface
    /// <summary>
    /// Tickerデータを溜めておいて一定時間ごとに登録する仕組みの管理
    /// </summary>
    public interface ITickerService
    {
        /// <summary>
        /// Tickerを受信するWebSocketを追加する
        /// </summary>
        /// <param name="myWebSocket">Tickerを受信するWebSocket</param>
        /// <param name="board">MBoard</param>
        /// <param name="timeScales">時間足リスト</param>
        void AddWebSocket(MyWebSocket myWebSocket, MBoard board, List<MTimeScale> timeScales);

        /// <summary>
        /// 指定したキーのWebSocketを取得する
        /// </summary>
        /// <param name="boardName">MBoardで設定した名前</param>
        /// <returns></returns>
        MyWebSocket GetWebSocket(string boardName);

        /// <summary>
        /// 全ての最新Tickerを取得する
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Ticker> GetAllTicker();
    }
    #endregion

    /// <summary>
    /// 本体
    /// </summary>
    public class TickerService : ITickerService
    {
        /// <summary>
        /// ログ
        /// </summary>
        private ILogger<TickerService> Logger { get; }

        /// <summary>
        /// Tickerを取得するWebSocket全て
        /// キーはMBoardsのName
        /// </summary>
        private Dictionary<string, MyWebSocket> WebSockets { get; }

        /// <summary>
        /// 時間足ごとのローソクを作成・保持するオブジェクト
        /// キーはMBoardsのName
        /// MyWebSocketに持たせないのは、MyWebSocketはTicker専用でないため
        /// </summary>
        private Dictionary<string, CandleMaker> CandleMakers { get; }

        /// <summary>
        /// データベース
        /// </summary>
        private ApplicationDbContext DbContext { get; }

        /// <summary>
        /// Ticker収集の管理を行う
        /// </summary>
        /// <param name="logger"></param>
        public TickerService(ILogger<TickerService> logger, ApplicationDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
            WebSockets = new Dictionary<string, MyWebSocket>();
            CandleMakers = new Dictionary<string, CandleMaker>();
            Logger.LogInformation("TickerService初期化完了");
        }

        /// <summary>
        /// WebSocket取得
        /// </summary>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public MyWebSocket GetWebSocket(string boardName)
        {
            return WebSockets[boardName];
        }

        /// <summary>
        /// Tickerを受信するWebSocketを追加する
        /// </summary>
        /// <param name="myWebSocket">Tickerを受信するWebSocket</param>
        /// <param name="board">MBoard</param>
        /// <param name="timeScales">時間足リスト、時間が短い順</param>
        public void AddWebSocket(MyWebSocket myWebSocket, MBoard board, List<MTimeScale> timeScales)
        {
            // ローソク作成クラスを作成する
            var candleMaker = CandleMaker.MakeGeneration(Logger,　DbContext, timeScales, board);

            // 受信時のイベント設定
            if (board.Name.StartsWith(SystemConstants.BoardPrefixBitflyer))
            {
                // BFの場合
                myWebSocket.GetMessage += (obj, e) => {
                    var textEvent = e as TextEventArgs;
                    var ticker = JsonConvert.DeserializeObject<LightningTicker>(textEvent.Text);
                    
                    // tickerをこのWSに対応したローソク作成クラスの一番親に送る
                    candleMaker.Update(new Ticker(ticker));
                };
            }

            // リストに追加
            CandleMakers.Add(board.Name, candleMaker);
            WebSockets.Add(board.Name, myWebSocket);
        }

        /// <summary>
        /// 全ての最新Tickerを取得する
        /// nullの場合もあるので注意
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Ticker> GetAllTicker()
        {
            var result = new Dictionary<string, Ticker>();
            foreach (var item in CandleMakers.Keys)
            {
                result.Add(item, CandleMakers[item].CurrentTicker);
            }
            return result;
        }
    }

}
