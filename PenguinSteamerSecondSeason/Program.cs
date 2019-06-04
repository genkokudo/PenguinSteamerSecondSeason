using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PenguinSteamerSecondSeason.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason
{
    class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            host.Run();
        }

        #region CreateWebHostBuilder
        /// <summary>
        /// マイグレーションのためにこのようなメソッドを作成する必要がある
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateWebHostBuilder(string[] args)
        {
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
                        .AddJsonFile($"appsettings.{envName}.json")
                        .AddEnvironmentVariables(prefix: SystemConstants.PrefixEnv)
                        .Build();
                })
                .ConfigureServices((services) =>
                {
                    // DBコンテキストを設定する、マイグレをする時はここを確認する
                    MakeDbContext(services, configuration, envName);

                    // サービス処理のDI
                    // ログ
                    services.AddLogging();
                    // テスト（シングルトンで追加する方法）
                    services.AddSingleton<IMyService, MyService>();
                    // テスト（呼び出すたびにインスタンス作成する方法）
                    //services.AddTransient<IMyService, MyService>();
                    // メインロジック
                    // IHostedServiceを実装すると、AddHostedServiceで指定することで動かせる。
                    services.AddHostedService<Application>();
                })
                .ConfigureLogging((context, config) =>
                {
                    // TODO:
                    // ここで設定すると、ILogger<各クラス名> loggerでインジェクションできる

                    //config.SetMinimumLevel(LogLevel.Debug);

                    //// Console ロガーの追加
                    //config.AddConsole();

                    //// NLog や Log4Net、SeriLog などを追加

                    //// あるいはApplication Insight の追加
                });
        }
        #endregion

        #region DBコンテキストを設定する
        /// <summary>
        /// DBコンテキストを設定する
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="envName"></param>
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
