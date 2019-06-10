using Microsoft.EntityFrameworkCore;
using PenguinSteamerSecondSeason.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason
{
    // データモデルを追加したとき、このクラスも更新すること
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MCurrency> MCurrencies { get; set; }
        public DbSet<MBoard> MBoards { get; set; }
        //public DbSet<MExchange> MExchanges { get; set; }
        public DbSet<MTimeScale> MTimeScales { get; set; }
        public DbSet<Candle> Candles { get; set; }

        /// <summary>
        /// 接続文字列などの設定はStartUpで作成したものをインジェクションする
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        #region 標準項目設定
        /// <summary>
        /// 登録・更新時に呼び出して
        /// 標準項目（更新日時とか）の設定を行う
        /// </summary>
        /// <param name="name">登録・更新者名</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> SaveChangesAsync(string name, CancellationToken cancellationToken = default)
        {
            // 保存時に日時を設定する
            SetCreatedDateTime(name);

            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetCreatedDateTime(string name)
        {
            DateTime now = DateTime.Now;

            // 追加エンティティのうち、IEntity を実装したものを抽出
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .OfType<IEntity>();

            foreach (var entity in entities)
            {
                entity.UpdatedDate = now;
                entity.UpdatedBy = name;
                if (entity.CreatedDate is null)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = name;
                }
            }
        }
        #endregion
    }
}
