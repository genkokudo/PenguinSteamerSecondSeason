using Microsoft.EntityFrameworkCore;
using PenguinSteamerSecondSeason.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinSteamerSecondSeason.Datas
{
    // データモデルを追加したとき、このクラスも更新すること
    class ApplicationDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // パスワードを環境変数から取得
            var password = Environment.GetEnvironmentVariable(SystemConstants.DbPasswordEnv);
            // 接続文字列の設定
            // マイグレするときは環境変数読めないのでここに直書きすること
            optionsBuilder.UseMySql($"Server=localhost;Database=penguin;User Id=ginpay;Password={password};");
        }
    }
}
