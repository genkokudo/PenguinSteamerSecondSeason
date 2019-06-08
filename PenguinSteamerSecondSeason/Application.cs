using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PenguinSteamerSecondSeason.Services;
using System;
using System.Collections.Generic;
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

        // 将来、IApplicationLifetimeは廃止され、IHostApplicationLifetimeになる
        private readonly IApplicationLifetime AppLifetime;
        IWebSocketService WebSocketService { get; }
        ILogger<Application> Logger { get; }
        public Application(IApplicationLifetime appLifetime, IWebSocketService webSocketService, ILogger<Application> logger)
        {
            // ここでインジェクションしたものをクラスフィールドに保持する
            AppLifetime = appLifetime;
            WebSocketService = webSocketService;
            Logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            AppLifetime.ApplicationStarted.Register(OnStarted); // 最低限これだけあればバッチ処理としては成り立つ

            Logger.LogInformation("開始処理");

            // WebSocket全部繋ぐ
            var task = InitializeWebSocketAsync(); // いつ終わってもいいのでawait待機しない

            return Task.CompletedTask;
        }

        /// <summary>
        /// DBに登録したWebSocketに全部接続する
        /// </summary>
        private Task InitializeWebSocketAsync()
        {
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
