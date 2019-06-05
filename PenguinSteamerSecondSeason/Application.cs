﻿using Microsoft.Extensions.Configuration;
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
        // 将来、IApplicationLifetimeは廃止され、IHostApplicationLifetimeになる
        private readonly IApplicationLifetime AppLifetime;
        IMyService MyService { get; }
        IConfiguration Configuration { get; }
        ILogger<Application> Logger { get; }
        public Application(IApplicationLifetime appLifetime, IMyService myService, IConfiguration configuration, ILogger<Application> logger)
        {
            // ここでインジェクションしたものをクラスフィールドに保持する
            AppLifetime = appLifetime;
            MyService = myService;
            Configuration = configuration;
            Logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            AppLifetime.ApplicationStarted.Register(OnStarted); // 最低限これだけあればバッチ処理としては成り立つ

            Logger.LogInformation("開始処理");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("終了処理");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            Logger.LogInformation("メインロジック開始");
            Logger.LogTrace("設定ファイルの内容を出力");
            Console.WriteLine(Configuration.GetValue<string>("aaaa:bbbb"));
            Console.WriteLine(Configuration.GetValue<string>("cccc"));
            // 実際の処理はここに書きます
            Logger.Log(LogLevel.Trace, "Trace");
            Logger.Log(LogLevel.Debug, "Debug");
            Logger.Log(LogLevel.Information, "Information");
            Logger.Log(LogLevel.Warning, "Warning");
            Logger.Log(LogLevel.Error, "Error");
            Logger.Log(LogLevel.Critical, "Critical");
            Logger.Log(LogLevel.None, "None");

            AppLifetime.StopApplication(); // 自動でアプリケーションを終了させる
        }
    }
}
