using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;
using NLog.Extensions.Logging;
using PenguinSteamerSecondSeason.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason
{
    /// <summary>
    /// このホスティングの方法だと、EFCoreのマイグレーションでエラーが出るため
    /// このようなFactoryクラスを準備する
    /// </summary>
    public class ContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=penguin;User Id=ginpay;Password=password;",
                mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(10, 3, 13), ServerType.MariaDb);
                }
            );
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }

    class Program
    {
        #region Main
        public static void Main(/* string[] args */)
        {
            // スタート！
            var logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                // ホストを作成して実行
                var host = CreateWebHostBuilder().Build();

                host.Run();
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
        #endregion

        #region CreateWebHostBuilder
        /// <summary>
        /// マイグレーションのためにこのようなメソッドを作成する必要がある
        /// ホストを作成する
        /// </summary>
        /// <returns></returns>
        private static IHostBuilder CreateWebHostBuilder()
        {
            // 作成した設定を保持
            IConfiguration configuration = null;

            // 環境変数から開発環境か本番系かを取得
            // hostContext.HostingEnvironment.EnvironmentNameだと何故か常にProductionになってしまう
            var envName = SystemConstants.EnvDevelopment;
            envName = Environment.GetEnvironmentVariable(SystemConstants.EnxEnv) ?? envName;   // マイグレーションでは無視される

            return new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    // 設定ファイルと環境変数を読み込んで保持
                    // ここで設定したらIConfiguration configurationでインジェクションできる
                    // 階層的な項目はconfiguration.GetValueの引数で、":"で区切って指定
                    // AddJsonFileで存在しなくても良い場合はoptional = trueを指定
                    configuration = configApp
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile("appsettings.Production.json", true)
                        .AddEnvironmentVariables(prefix: SystemConstants.PrefixEnv)
                        .Build();
                })
                .ConfigureServices((services) =>
                {
                    // DBコンテキストを設定する
                    MakeDbContext(services, configuration, envName);

                    // サービス処理のDI
                    // ログ
                    services.AddLogging();
                    // WebSocket管理（シングルトンで追加する方法）
                    services.AddSingleton<ITickerService, TickerService>();
                    // Ticker登録（インジェクションごとにインスタンス作成する方法）
                    //services.AddTransient<ITickerService, TickerService>();
                    // メインロジック
                    // IHostedServiceを実装したクラスは、AddHostedServiceで指定することで自動起動する。
                    services.AddHostedService<WebSocketService>();
                    services.AddHostedService<Application>();
                })
                .ConfigureLogging((context, config) =>
                {
                    // ここで設定すると、ILogger<各クラス名> loggerでインジェクションできる
                    // NLogを追加
                    config.ClearProviders();
                    config.SetMinimumLevel(LogLevel.Trace);  //これがないと正常動作しない模様
                })
                // NLog を有効にする
                .UseNLog();
        }
        #endregion

        #region DBコンテキストを作成して、サービスに追加する
        /// <summary>
        /// DBコンテキストを作成して、サービスに追加する
        /// 環境によって分岐
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="envName">DevelopmentかProduction</param>
        private static void MakeDbContext(IServiceCollection services, IConfiguration configuration, string envName)
        {
            // appsettings.jsonから、使用するデータベースの接続文字列設定を取得
            if (envName == SystemConstants.EnvDevelopment)
            {
                // 開発系はappsettingsから接続文字列とパスワードを取得する。
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString(SystemConstants.Connection),
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(10, 3, 13), ServerType.MariaDb);
                    }
                ));
            }
            else
            {
                // 本番系は接続先をappsettingsから、パスワードを環境変数から取得する
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString(SystemConstants.Connection) + "Password=" + configuration.GetValue<string>(SystemConstants.DbPasswordEnv) + ";",
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(10, 3, 13), ServerType.MariaDb);
                    }
                ));
            }
        }
        #endregion

    }
}
