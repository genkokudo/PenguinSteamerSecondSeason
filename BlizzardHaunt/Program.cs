using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlizzardHaunt.Infrastructure.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Penguinium;

namespace BlizzardHaunt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Webホスト作成
            var host = CreateWebHostBuilder(args).Build();

            // DBに初期値を登録
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // DBをマイグレーションする
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();
                    // マスタデータの初期化が必要な場合、こういうクラスを作成する
                    //DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        /// <summary>
        /// WebHostを作成します。
        /// 3.0以降の書き方
        /// </summary>
        /// <param name="args"></param>
        /// <param name="port">使用するポート</param>
        /// <returns></returns>
        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                        // NLog 以外で設定された Provider の無効化.
                        logging.ClearProviders();
                        // 最小ログレベルの設定.
                        logging.SetMinimumLevel(LogLevel.Trace);
                })
            //// NLog を有効にする.
            //.UseNLog()
            ;
    }


}
