using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PenguinSteamerSecondSeason.Models;
using PenguinSteamerSecondSeason.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason
{
    /// <summary>
    /// メインロジック
    /// </summary>
    class Application : IHostedService
    {
        const string BtcJpy = "lightning_ticker_BTC_JPY";
        const string FxBtcJpy = "lightning_ticker_FX_BTC_JPY";
        const string EndPoint = "wss://ws.lightstream.bitflyer.com/json-rpc";

        #region Field, Initialize
        // 将来、IApplicationLifetimeは廃止され、IHostApplicationLifetimeになる
        /// <summary>
        /// シャットダウン中にクリーンアップ出来るようにする仕組み
        /// </summary>
        private readonly IApplicationLifetime AppLifetime;

        /// <summary>
        /// 全てのWebSocketをここで管理する
        /// </summary>
        IWebSocketService WebSocketService { get; }
        /// <summary>
        /// ログ
        /// </summary>
        ILogger<Application> Logger { get; }
        /// <summary>
        /// 設定ファイルの読み込み
        /// </summary>
        IConfiguration Configuration { get; }
        /// <summary>
        /// Tickerのデータを溜めて、DBを更新する
        /// </summary>
        ITickerUpdateService TickerUpdateService { get; }

        /// <summary>
        /// データベース
        /// </summary>
        private ApplicationDbContext DbContext { get; }
        public Application(IApplicationLifetime appLifetime, IConfiguration configuration, IWebSocketService webSocketService, ILogger<Application> logger, ITickerUpdateService tickerUpdateService, ApplicationDbContext dbContext)
        {
            // ここでインジェクションしたものをクラスフィールドに保持する
            AppLifetime = appLifetime;
            WebSocketService = webSocketService;
            Logger = logger;
            TickerUpdateService = tickerUpdateService;
            DbContext = dbContext;
            Configuration = configuration;
        }
        #endregion

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            AppLifetime.ApplicationStarted.Register(OnStarted); // 最低限これだけあればバッチ処理としては成り立つ

            Logger.LogInformation("開始処理");

            // DB確認して、データが無ければ初期データを入れる
            await InitializeDatabaseAsync();

            // WebSocket全部繋ぐ
            var task = InitializeWebSocketAsync(); // いつ終わってもいいのでawait待機しない
        }

        #region InitializeDatabaseAsync:DB確認して、データが無ければ初期データを入れる

        /// <summary>
        /// DB確認して、データが無ければ初期データを入れる
        /// </summary>
        /// <returns></returns>
        private async Task InitializeDatabaseAsync()
        {
            // DB確認して、データが無ければ初期データを入れる
            if (DbContext.MCurrencies.ToList().Count == 0)
            {
                Logger.LogInformation("通貨データがありません。初期値を登録します。");
                var array = new List<string[]>();
                var section = Configuration.GetSection("DefaultParameters");
                section.Bind("MCurrency", array);
                foreach (var item in array)
                {
                    var data = new MCurrency
                    {
                        Id = int.Parse(item[0]),
                        Name = item[1],
                        DisplayName = item[2]
                    };
                    DbContext.MCurrencies.Add(data);
                }
                await DbContext.SaveChangesAsync(SystemConstants.SystemName);
            }

            if (DbContext.MBoards.ToList().Count == 0)
            {
                Logger.LogInformation("板データがありません。初期値を登録します。");
                var array = new List<string[]>();
                var section = Configuration.GetSection("DefaultParameters");
                section.Bind("MBoard", array);
                foreach (var item in array)
                {
                    var data = new MBoard
                    {
                        Id = int.Parse(item[0]),
                        Name = item[1],
                        DisplayName = item[2],
                        Currency1 = new MCurrency { Id = int.Parse(item[3]) },
                        Currency2 = new MCurrency { Id = int.Parse(item[4]) }
                    };
                    DbContext.MBoards.Add(data);
                }
                await DbContext.SaveChangesAsync(SystemConstants.SystemName);
            }

            if (DbContext.MTimeScales.ToList().Count == 0)
            {
                Logger.LogInformation("時間足データがありません。初期値を登録します。");
                var array = new List<string[]>();
                var section = Configuration.GetSection("DefaultParameters");
                section.Bind("MTimeScale", array);
                foreach (var item in array)
                {
                    var data = new MTimeScale
                    {
                        Id = int.Parse(item[0]),
                        DisplayName = item[1],
                        SecondsValue = int.Parse(item[2])
                    };
                    DbContext.MTimeScales.Add(data);
                }
                await DbContext.SaveChangesAsync(SystemConstants.SystemName);
            }
        }
        #endregion

        /// <summary>
        /// DBに登録したWebSocketに全部接続する
        /// </summary>
        private Task InitializeWebSocketAsync()
        {
            // TODO:Ticker
            TickerUpdateService.Run();

            Logger.LogInformation("WebSocket接続を開始します");
            // 対象をサービスに登録していく
            // TODO:対象はDBから読むがいい
            WebSocketService.Add(EndPoint, BtcJpy);
            WebSocketService.Add(EndPoint, FxBtcJpy);

            // 登録した対象全て開始
            return WebSocketService.StartAllAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("終了処理");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            Logger.LogInformation("メインロジック開始");
            //Logger.LogTrace("設定ファイルの内容を出力");
            //Console.WriteLine(Configuration.GetValue<string>("aaaa:bbbb"));
            //Console.WriteLine(Configuration.GetValue<string>("cccc"));
            //// 実際の処理はここに書きます
            //Logger.Log(LogLevel.Trace, "Trace");
            //Logger.Log(LogLevel.Debug, "Debug");
            //Logger.Log(LogLevel.Information, "Information");
            //Logger.Log(LogLevel.Warning, "Warning");
            //Logger.Log(LogLevel.Error, "Error");
            //Logger.Log(LogLevel.Critical, "Critical");
            //Logger.Log(LogLevel.None, "None");

            //AppLifetime.StopApplication(); // 自動でアプリケーションを終了させる
        }
    }
}
