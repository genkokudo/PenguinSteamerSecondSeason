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
        #region Field, Initialize
        // 将来、IApplicationLifetimeは廃止され、IHostApplicationLifetimeになる
        /// <summary>
        /// シャットダウン中にクリーンアップ出来るようにする仕組み
        /// </summary>
        private IApplicationLifetime AppLifetime { get; }

        /// <summary>
        /// ログ
        /// </summary>
        private ILogger<Application> Logger { get; }

        /// <summary>
        /// 設定ファイルの読み込み
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// データベース
        /// </summary>
        private ApplicationDbContext DbContext { get; }
        public Application(IApplicationLifetime appLifetime, IConfiguration configuration, ILogger<Application> logger, ApplicationDbContext dbContext)
        {
            // ここでインジェクションしたものをクラスフィールドに保持する
            AppLifetime = appLifetime;
            Logger = logger;
            DbContext = dbContext;
            Configuration = configuration;
            Logger.LogInformation("Application初期化完了");
        }
        #endregion

        #region 開始処理
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Application開始処理");

            // DB確認して、データが無ければ初期データを入れる
            await InitializeDatabaseAsync();

            AppLifetime.ApplicationStarted.Register(OnStarted); // 最低限これだけあればバッチ処理としては成り立つ
        }
        #endregion

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

            //if (DbContext.MWebSockets.ToList().Count == 0)
            //{
            //    Logger.LogInformation("WebSocketデータがありません。初期値を登録します。");
            //    var array = new List<string[]>();
            //    var section = Configuration.GetSection("DefaultParameters");
            //    section.Bind("MWebSocket", array);
            //    foreach (var item in array)
            //    {
            //        var data = new MWebSocket
            //        {
            //            Id = int.Parse(item[0]),
            //            Board = new MBoard { Id = int.Parse(item[1]) },
            //            Category = int.Parse(item[2]),
            //            EndPoint = item[3],
            //            ChannelName = item[4],
            //            IsEnabled = int.Parse(item[5]) == 1
            //        };
            //        DbContext.MWebSockets.Add(data);
            //    }
            //    await DbContext.SaveChangesAsync(SystemConstants.SystemName);
            //}
        }
        #endregion

        #region 終了処理
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Application終了処理");

            return Task.CompletedTask;
        }
        #endregion

        private void OnStarted()
        {
            Logger.LogInformation("Applicationメインロジック開始");
            // 実際の処理はここに書きます

            //AppLifetime.StopApplication(); // 自動でアプリケーションを終了させる
        }
    }
}
