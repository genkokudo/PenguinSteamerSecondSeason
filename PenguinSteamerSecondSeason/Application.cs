using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
        public Application(IApplicationLifetime appLifetime, IMyService myService, IConfiguration configuration)
        {
            // ここでインジェクションしたものをクラスフィールドに保持する
            AppLifetime = appLifetime;
            MyService = myService;
            Configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            AppLifetime.ApplicationStarted.Register(OnStarted); // 最低限これだけあればバッチ処理としては成り立つ

            Console.WriteLine("StartAsync");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("StopAsync");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            Console.WriteLine("メインロジックやで。");
            Console.WriteLine("設定ファイル");
            Console.WriteLine(Configuration.GetValue<string>("aaaa:bbbb"));
            Console.WriteLine(Configuration.GetValue<string>("cccc"));
            Console.WriteLine("設定ファイルここまで");
            // 実際の処理はここに書きます

            AppLifetime.StopApplication(); // 自動でアプリケーションを終了させる
        }
    }
}
