using Microsoft.Extensions.Logging;
using PenguinSteamerSecondSeason.Common;
using PenguinSteamerSecondSeason.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason.Services
{
    // 出来れば溜める場所は複数作って、このサービスはそれら全部の管理をやって貰いたい
    /// <summary>
    /// Tickerデータを溜めておいて一定時間ごとに登録する仕組みの管理
    /// </summary>
    public interface ITickerUpdateService
    {
        void Run();
    }

    /// <summary>
    /// 本体
    /// </summary>
    public class TickerUpdateService : ITickerUpdateService
    {
        /// <summary>
        /// ログ
        /// </summary>
        ILogger<TickerUpdateService> Logger { get; }

        /// <summary>
        /// 全てのTickerの更新用モジュール
        /// TODO:外からアクセスするならDictionaryにすべきじゃないの？
        /// </summary>
        List<MyTickerUpdate> TickerUpdate { get; }

        /// <summary>
        /// データベース
        /// </summary>
        ApplicationDbContext DbContext { get; }

        /// <summary>
        /// Ticker収集の管理を行う
        /// </summary>
        /// <param name="logger"></param>
        public TickerUpdateService(ILogger<TickerUpdateService> logger, ApplicationDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
            TickerUpdate = new List<MyTickerUpdate>();
            // TickerUpdateの登録をする
            // TODO:対象はDBから読み込みたまえ。
        }

        /// <summary>
        /// 全てのTicker更新を動かす
        /// </summary>
        public void Run()
        {
            foreach (var item in TickerUpdate)
            {
                // TODO:
            }
        }
    }

}
