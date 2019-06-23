using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PenguinSteamerSecondSeason.Common;
using PenguinSteamerSecondSeason.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason.Services
{
    /// <summary>
    /// 複数のWebSocketを管理する
    /// 現在の所、DBから対象を読み込んで繋いで走らせるだけ。TickerはTicker管理サービスに投げる。
    /// 走っているWebSocketには外からアクセスできないが、Ticker以外を扱うときに再検討する。
    /// 受信したTicker情報はTickerServiceから参照できる。
    /// </summary>
    public class WebSocketService :  IHostedService
    {
        #region Field, Initialize
        /// <summary>
        /// シャットダウン中にクリーンアップ出来るようにする仕組み
        /// </summary>
        private IApplicationLifetime AppLifetime { get; }

        /// <summary>
        /// 使用しているWebSocket
        /// </summary>
        private List<MyWebSocket> MyWebSockets { get; }

        /// <summary>
        /// ログ
        /// </summary>
        private ILogger<WebSocketService> Logger { get; }

        /// <summary>
        /// データベース
        /// </summary>
        private ApplicationDbContext DbContext { get; }

        /// <summary>
        /// 設定ファイルの読み込み
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Ticker管理
        /// </summary>
        private ITickerService TickerService { get; }
        /// <summary>
        /// 複数のWebSocketを管理する
        /// </summary>
        public WebSocketService(IApplicationLifetime appLifetime, ILogger<WebSocketService> logger, IConfiguration configuration, ApplicationDbContext dbContext, ITickerService tickerService)
        {
            MyWebSockets = new List<MyWebSocket>();
            Logger = logger;
            DbContext = dbContext;
            Configuration = configuration;
            AppLifetime = appLifetime;
            TickerService = tickerService;
        }

        #endregion

        #region 開始処理
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("IWebSocketService開始処理");

            // WebSocket全部繋ぐ
            // TickerならばTicker管理サービスに投げる
            InitializeWebSocket();

            // 本編開始
            AppLifetime.ApplicationStarted.Register(OnStarted);

            return Task.CompletedTask;
        }

        /// <summary>
        /// DBに登録したWebSocketに全部接続する
        /// </summary>
        private void InitializeWebSocket()
        {
            Logger.LogInformation("WebSocket接続を開始します");
            // DBから作成する時間足を取得する、現在は全板共通、時間が短い順に取得
            var timeScales = DbContext.MTimeScales.OrderBy(d => d.SecondsValue).ToList();

            // 対象をDBから取得して、サービスに登録していく
            var wss = DbContext.MWebSockets.Include(x => x.MBoard).Where(d => d.IsEnabled && !d.IsDeleted).ToList();

            foreach (var item in wss)
            {
                // WebSocket作成・追加
                var ws = Add(item.EndPoint, item.ChannelName);
                if(item.Category == 1)
                {
                    // Tickerの場合、Ticker管理サービスに登録して外からアクセスできるようにする
                    TickerService.AddWebSocket(ws, item.MBoard, timeScales);
                }
            }
        }

        /// <summary>
        /// 扱うWebSocket追加
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="channelName"></param>
        /// <returns></returns>
        private MyWebSocket Add(string endPoint, string channelName)
        {
            var myWebSocket = new MyWebSocket(endPoint, channelName, Logger);
            Logger.LogInformation($"WebSocket追加:{myWebSocket.ToString()}");
            MyWebSockets.Add(myWebSocket);
            return myWebSocket;
        }
        #endregion

        #region 終了処理、特に何も行わない
        /// <summary>
        /// 終了処理
        /// 特に何も行わない
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("IWebSocketService終了処理");

            return Task.CompletedTask;
        }
        #endregion

        #region メイン処理
        private void OnStarted()
        {
            Logger.LogInformation("IWebSocketServiceメインロジック開始");

            // 登録した対象全て開始
            // あとは登録されているMyWebSocketが自発的に動作し、このサービスは干渉しない。
            StartAll();
        }

        /// <summary>
        /// 登録したWebSocketを全て開始
        /// </summary>
        /// <returns></returns>
        private void StartAll()
        {
            Logger.LogInformation("WebSocketを全て接続開始します");
            // WebSocket全部繋ぎます
            var list = new List<Task>();
            foreach (var item in MyWebSockets)
            {
                Logger.LogInformation($"WebSocket接続:{item.ToString()}");
                list.Add(item.RunAsync());
            }
        }
        #endregion
    }

}
