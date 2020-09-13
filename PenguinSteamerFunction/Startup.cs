using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenguinSteamerFunction;
using PenguinSteamerFunction.Infrastructure.Database;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace PenguinSteamerFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // DBのセットアップ
            builder.Services.AddDbContextPool<AppDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString")));

            // 設定値の読み込み
            builder.Services.AddOptions<UserSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.Bind("UserSettings", settings));
        }
    }
}
