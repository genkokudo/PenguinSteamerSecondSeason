using Microsoft.Extensions.Logging;
using PenguinSteamerSecondSeason.Common;
using PenguinSteamerSecondSeason.Data;
using PenguinSteamerSecondSeason.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason.Services
{
    // 繋いだWebSocketのうち、Tickerだけを集めて辞書で管理することで、外からアクセスできるようにする。
    // 各Tickerの最新の値を持っておく（というか、アクセスできるようにする）
    // メソッドで↑を一覧表示する

    /// <summary>
    /// Tickerデータを溜めておいて一定時間ごとに登録する仕組みの管理
    /// </summary>
    public interface ITickerService
    {
        void AddWebSocket(MyWebSocket myWebSocket, string boardName);

        /// <summary>
        /// 指定したキーのWebSocketを取得する
        /// </summary>
        /// <param name="boardName">MBoardで設定した名前</param>
        /// <returns></returns>
        MyWebSocket GetWebSocket(string boardName);

        ///// <summary>
        ///// 取り敢えず全Tickerの最新
        ///// TODO:文字列じゃなくてTickerのリストで返すべき
        ///// </summary>
        ///// <returns></returns>
        //string All();
    }

    /// <summary>
    /// 本体
    /// </summary>
    public class TickerService : ITickerService
    {
        /// <summary>
        /// ログ
        /// </summary>
        ILogger<TickerService> Logger { get; }

        /// <summary>
        /// 全てのTicker
        /// </summary>
        Dictionary<string, MyWebSocket> WebSocket { get; }

        /// <summary>
        /// データベース
        /// </summary>
        ApplicationDbContext DbContext { get; }

        /// <summary>
        /// Ticker収集の管理を行う
        /// </summary>
        /// <param name="logger"></param>
        public TickerService(ILogger<TickerService> logger, ApplicationDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
            WebSocket = new Dictionary<string, MyWebSocket>();
            Logger.LogInformation("TickerService初期化完了");
        }

        /// <summary>
        /// WebSocket取得
        /// </summary>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public MyWebSocket GetWebSocket(string boardName)
        {
            return WebSocket[boardName];
        }

        /// <summary>
        /// Tickerを受信するWebSocketを追加する
        /// </summary>
        /// <param name="myWebSocket">Tickerを受信するWebSocket</param>
        /// <param name="boardName">MBoardで設定した名前</param>
        public void AddWebSocket(MyWebSocket myWebSocket, string boardName)
        {
            // TODO:ローソク作成クラスを作成する

            // 受信時のイベント設定
            if (boardName.StartsWith(SystemConstants.BoardPrefixBitflyer))
            {
                // BFの場合
                // TODO:変換するクラスを指定する
                //var s = JsonConvert.DeserializeObject<LightningTicker>(string);
            }
            WebSocket.Add(boardName, myWebSocket);
        }

        public Dictionary<string, MyWebSocket> GetAllTicker()
        {
            // TODO:キーと最新Tickerの辞書を作って返すべき
            // Dictionary<string, Ticker>
            return WebSocket;
        }
    }

}
