﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlizzardHaunt.Data;
using Microsoft.EntityFrameworkCore;
using Penguinium.Infrastructure.Database;

namespace BlizzardHaunt
{
    public class Startup
    {
        /// <summary>
        /// 環境変数を取得するのに使用
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// 設定ファイルの値取得
        /// </summary>
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            //構成ファイル、環境変数等から、構成情報をロード
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            //構成情報をプロパティに設定
            Configuration = builder.Build();    // IConfiguration configurationをインジェクションしない
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // 環境変数から接続文字列とパスワードを取得する。
            // SQLCONNSTR_BHで接続文字列を環境変数に設定。Azureのアプリケーション設定の接続文字列にも"BH"として設定すること。
            var constr = Configuration.GetConnectionString("BH");
            if (string.IsNullOrWhiteSpace(constr))
            {
                // 結局、EFCoreのToolは環境変数を読むことはできない（ASPNETCORE_ENVIRONMENTはappsettings.jsonを読む時に認識する）
                // マイグレーションの生成の時用に、仮の接続文字列をハードコードかappsettings.Development.jsonに書くのが正解。
                constr = Configuration.GetValue<string>("ConnectionStringForMigration");
            }
            // DBの設定
            // クラスライブラリにDbContextを作成する場合、MigrationsAssemblyを指定する必要がある。
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(constr, b => b.MigrationsAssembly("BlizzardHaunt"))
                );
            
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // https://aka.ms/aspnetcore-hsts
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
