using ChainingAssertion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PenguinSteamerSecondSeason;
using PenguinSteamerSecondSeason.Common;
using PenguinSteamerSecondSeason.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace PenguinSteamerTest
{
    public class CandleMakerTest
    {
        /// <summary>
        /// ログ
        /// </summary>
        private readonly ILogger Logger;

        /// <summary>
        /// デバッグメッセージの出力
        /// </summary>
        private readonly ITestOutputHelper Output;

        #region 固定データ
        /// <summary>
        /// テストデータ
        /// 時間足
        /// </summary>
        private static readonly List<MTimeScale> TimeScales = new List<MTimeScale>
            {
                new MTimeScale() { Id = 1, DisplayName = "1分", SecondsValue = 60 },
                new MTimeScale() { Id = 2, DisplayName = "3分", SecondsValue = 180 },
                new MTimeScale() { Id = 3, DisplayName = "5分", SecondsValue = 300 },
                new MTimeScale() { Id = 4, DisplayName = "10分", SecondsValue = 600 },
                new MTimeScale() { Id = 5, DisplayName = "30分", SecondsValue = 1800 },
                new MTimeScale() { Id = 6, DisplayName = "1時間", SecondsValue = 3600 },
                new MTimeScale() { Id = 7, DisplayName = "2時間", SecondsValue = 7200 },
                new MTimeScale() { Id = 8, DisplayName = "4時間", SecondsValue = 14400 },
                new MTimeScale() { Id = 9, DisplayName = "12時間", SecondsValue = 43200 },
                new MTimeScale() { Id = 10, DisplayName = "1日", SecondsValue = 86400 }
            };

        /// <summary>
        /// テストデータ
        /// 板
        /// ※通貨設定してないので他のテストで使うとき注意
        /// </summary>
        private static readonly List<MBoard> Boards = new List<MBoard>
            {
                new MBoard() { Id = 11, Name = "BITFLYER_BTC_JPY", DisplayName = "bitFlyer現物BTC" },
                new MBoard() { Id = 12, Name = "BITFLYER_FX_BTC_JPY", DisplayName = "bitFlyerFX" },
                new MBoard() { Id = 13, Name = "BITFLYER_ETH_BTC", DisplayName = "bitFlyer現物ETH" },
                new MBoard() { Id = 14, Name = "BITFLYER_BCH_BTC", DisplayName = "bitFlyer現物BCH" }
            };

        /// <summary>
        /// テストデータ
        /// 通貨
        /// </summary>
        private static readonly List<MCurrency> Currencies = new List<MCurrency>
            {
                new MCurrency() { Id = 1, Name = "JPY", DisplayName = "円" },
                new MCurrency() { Id = 2, Name = "BTC", DisplayName = "BTC" },
                new MCurrency() { Id = 3, Name = "ETH", DisplayName = "Ethereum" },
                new MCurrency() { Id = 4, Name = "BCH", DisplayName = "Bitcoin Cash" }
            };
        #endregion

        #region テストデータ
        public static IEnumerable<object[]> CandleMakerTestDataProp
        {
            get
            {
                yield return new object[] { TimeScales, Boards[0] };
                yield return new object[] { TimeScales, Boards[1] };
            }
        }
        #endregion

        public CandleMakerTest(ITestOutputHelper output)
        {
            Output = output;

            // ロガー作成
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>().AddNLog();

            Logger = factory.CreateLogger<CandleMakerTest>();
        }

        [Theory(DisplayName = "CandleMaker作成")]
        [MemberData(nameof(CandleMakerTestDataProp))]
        [Trait("Category", "CandleMakerの作成")]
        public void Test1(List<MTimeScale> timeScales, MBoard board)
        {
            // InMemoryDBを使うオプションを作成
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            CandleMaker data = null;
            using (var dbContext = new ApplicationDbContext(options)) // InMemoryDBを使うオプション設定を渡す
            {
                // インスタンス作成
                data = CandleMaker.MakeGeneration(Logger, dbContext, timeScales, board);
            }

            // 名前の確認
            data.DisplayName.Is("1分");

            // この時間足データの場合、一番親の子が2つあること
            data.Children.Count.Is(2);

            // 約数があって一番大きい秒数の子が親から辿れること
            // 表示名が正しく設定されていること
            data.Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].DisplayName.Is("1日");
        }

        #region xUnitの基本機能
        [Fact(DisplayName = "xUnitでNLogを使用する")]
        [Trait("Category", "xUnitの基本機能")]
        public void HelloLogger()
        {
            Logger.LogTrace("World Trace");
            Logger.LogDebug("World Debug");
            Logger.LogWarning("World Warn");
            Logger.LogError("★★World Error★★");
        }

        [Fact(DisplayName = "初めてのInMemoryDatabase")]
        [Trait("Category", "xUnitの基本機能")]
        public void AddDatabase()
        {
            // InMemoryDBを使うオプションを作成
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // 何かデータを追加。using ステートメントで一度 DB との接続を切る。
            using (var context = new ApplicationDbContext(options)) // InMemoryDBを使うオプション設定を渡す
            {
                foreach (var item in Currencies)
                {
                    context.MCurrencies.Add(item);
                }

                context.SaveChanges(SystemConstants.SystemName);
            }
            // 切断

            // 再接続してInMemoryDBから値を取り出す
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(4, context.MCurrencies.Count());
                var currencie = context.MCurrencies.First(e => e.Id == 1);
                Assert.Equal("JPY", currencie.Name);
                Assert.Equal(SystemConstants.SystemName, currencie.CreatedBy);
                Assert.Equal(SystemConstants.SystemName, currencie.UpdatedBy);
            }
        }
        #endregion
    }
}

///// <summary>
///// 最新のTicker
///// 値を持つのは一番親のみ
///// 未取得の場合もnull
///// </summary>
//public Ticker CurrentTicker { get; private set; }

///// <summary>
///// 最新のローソク（作成途中のローソク）
///// 完成したローソクはDBへ
///// </summary>
//public Candle CurrentCandle { get; private set; }

///// <summary>
///// 今まで作ったローソク
///// </summary>
//public List<Candle> CandleList { get; }

///// <summary>
///// 時間足
///// </summary>
//public MTimeScale TimeScale { get; }

///// <summary>
///// どの板か
///// </summary>
//public MBoard Board { get; }

//Update(Ticker ticker)
//        Tickerでローソクを更新する
//        時間が過ぎていたらローソク更新。
//        大きく時間が過ぎていたら複数本ローソク更新。
        
//        時間が過ぎていたら子を更新する

//        最大データ数を超えていたら古いデータを削除する※データ生成が必要
        
//        ■Candleクラス
//        Tickerによってローソクを更新する
//        ローソクデータによってローソクを更新する