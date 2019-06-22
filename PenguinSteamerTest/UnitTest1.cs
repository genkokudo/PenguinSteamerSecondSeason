using ChainingAssertion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PenguinSteamerSecondSeason;
using PenguinSteamerSecondSeason.Common;
using PenguinSteamerSecondSeason.Data;
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
        /// ���O
        /// </summary>
        private readonly ILogger Logger;

        /// <summary>
        /// �f�o�b�O���b�Z�[�W�̏o��
        /// </summary>
        private readonly ITestOutputHelper Output;

        #region �Œ�f�[�^
        /// <summary>
        /// �e�X�g�f�[�^
        /// ���ԑ�
        /// </summary>
        private static readonly List<MTimeScale> TimeScales = new List<MTimeScale>
            {
                new MTimeScale() { Id = 1, DisplayName = "1��", SecondsValue = 60 },
                new MTimeScale() { Id = 2, DisplayName = "3��", SecondsValue = 180 },
                new MTimeScale() { Id = 3, DisplayName = "5��", SecondsValue = 300 },
                new MTimeScale() { Id = 4, DisplayName = "10��", SecondsValue = 600 },
                new MTimeScale() { Id = 5, DisplayName = "30��", SecondsValue = 1800 },
                new MTimeScale() { Id = 6, DisplayName = "1����", SecondsValue = 3600 },
                new MTimeScale() { Id = 7, DisplayName = "2����", SecondsValue = 7200 },
                new MTimeScale() { Id = 8, DisplayName = "4����", SecondsValue = 14400 },
                new MTimeScale() { Id = 9, DisplayName = "12����", SecondsValue = 43200 },
                new MTimeScale() { Id = 10, DisplayName = "1��", SecondsValue = 86400 }
            };

        /// <summary>
        /// �e�X�g�f�[�^
        /// ��
        /// ���ʉݐݒ肵�ĂȂ��̂ő��̃e�X�g�Ŏg���Ƃ�����
        /// </summary>
        private static readonly List<MBoard> Boards = new List<MBoard>
            {
                new MBoard() { Id = 11, Name = "BITFLYER_BTC_JPY", DisplayName = "bitFlyer����BTC" },
                new MBoard() { Id = 12, Name = "BITFLYER_FX_BTC_JPY", DisplayName = "bitFlyerFX" },
                new MBoard() { Id = 13, Name = "BITFLYER_ETH_BTC", DisplayName = "bitFlyer����ETH" },
                new MBoard() { Id = 14, Name = "BITFLYER_BCH_BTC", DisplayName = "bitFlyer����BCH" }
            };

        /// <summary>
        /// �e�X�g�f�[�^
        /// �ʉ�
        /// </summary>
        private static readonly List<MCurrency> Currencies = new List<MCurrency>
            {
                new MCurrency() { Id = 1, Name = "JPY", DisplayName = "�~" },
                new MCurrency() { Id = 2, Name = "BTC", DisplayName = "BTC" },
                new MCurrency() { Id = 3, Name = "ETH", DisplayName = "Ethereum" },
                new MCurrency() { Id = 4, Name = "BCH", DisplayName = "Bitcoin Cash" }
            };
        #endregion

        #region �g�����U�N�V�����e�X�g�f�[�^

        /// <summary>
        /// 1�����pTicker�e�X�g�p�f�[�^
        /// </summary>
        private static readonly List<Ticker> Tickers1m = new List<Ticker>
            {
                // Ticker����͂��Ă���
                // �ŏ���Ticker�����͂��ꂽ��A���[�\�N���쐬�����
                // 1�����Ȃ̂ŁA�J�n�ƏI�����Ԃ�1�����Ƃɐ؂�̂�
                new Ticker() { Id = 1,
                    BestAsk = 801000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 800000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 800000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:22:16"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                new Ticker() { Id = 2,
                    BestAsk = 802000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 800000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 802000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:22:18"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                // 1���o�߂��Ă��Ȃ��ꍇ�A���[�\�N���X�g�ɉ����Ȃ��i���E�l�e�X�g�j
                new Ticker() { Id = 3,
                    BestAsk = 802000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 799000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:22:59"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                // 1���o�߂����烍�[�\�N���X�g�ɉ�����
                // 1���o�߂������Ticker���i�̓��[�\�N���X�g�̃��[�\�N�ɉe�����Ȃ�
                // 1���o�߂������Ticker���i�͐V�������[�\�N�ɔ��f����
                // 1���o�߂����烍�[�\�N��DB�ɓo�^����
                new Ticker() { Id = 4,
                    BestAsk = 803000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 803000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:23:00"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                // 5���o�߂����烍�[�\�N5�{�ɂȂ��Ă���ADB�ɑS�ēo�^����Ă���
                // �Ԃ̃��[�\�N��Min = Max�ɂȂ��Ă���
                new Ticker() { Id = 5,
                    BestAsk = 805000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 805000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:27:00"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                // 101���o�߂����烁�����̍ŏ��̃��[�\�N���폜����
                new Ticker() { Id = 6,
                    BestAsk = 806000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 806000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 09:03:00"),
                    Volume = 120000, VolumeByProduct = 120000
                }
            };

        /// <summary>
        /// 5�����pTicker�e�X�g�p�f�[�^
        /// </summary>
        private static readonly List<Ticker> Tickers5m = new List<Ticker>
            {
                // �e��Ticker����͂��Ă���
                // �ŏ���Ticker�����͂��ꂽ��A���[�\�N���쐬�����
                // 5�����Ȃ̂ŁA�J�n�ƏI�����Ԃ�5�����Ƃɐ؂�̂�
                new Ticker() { Id = 1,
                    BestAsk = 801000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 800000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 800000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:22:16"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                // 1���o�߂����烍�[�\�N���X�V�����
                new Ticker() { Id = 2,
                    BestAsk = 802000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 800000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 802000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:23:00"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                // 5���o�߂��Ă��Ȃ��ꍇ�A���[�\�N���X�g�ɉ����Ȃ��i���E�l�e�X�g�j
                new Ticker() { Id = 3,
                    BestAsk = 802000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 799000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:24:59"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                // 5���o�߂����烍�[�\�N���X�g�ɉ�����
                // 5���o�߂������Ticker���i�̓��[�\�N���X�g�̃��[�\�N�ɉe�����Ȃ�
                // 5���o�߂������Ticker���i�͐V�������[�\�N�ɔ��f����
                // 5���o�߂����烍�[�\�N��DB�ɓo�^����
                new Ticker() { Id = 4,
                    BestAsk = 803000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 803000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:25:00"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                // 15���o�߂����烍�[�\�N3�{�ɂȂ��Ă���ADB�ɑS�ēo�^����Ă���
                // �Ԃ̃��[�\�N��Min = Max�ɂȂ��Ă���
                new Ticker() { Id = 5,
                    BestAsk = 805000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 805000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 07:35:00"),
                    Volume = 120000, VolumeByProduct = 120000
                },
                // 505���o�߂�����DB�̍ŏ��̃��[�\�N���폜����
                new Ticker() { Id = 6,
                    BestAsk = 806000, BestAskSize = 0.01M, TotalAskDepth = 5000,
                    BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
                    Ltp = 806000,
                    TickId = 100,
                    Timestamp = DateTime.Parse("08/18/2018 15:45:00"),
                    Volume = 120000, VolumeByProduct = 120000
                }
            };

        #endregion

        #region �e�X�g�f�[�^
        /// <summary>
        /// CandleMaker�����̃e�X�g�f�[�^
        /// ��Theory�̗���p�ɍ쐬�����A�{����Fact�̕����K��
        /// </summary>
        public static IEnumerable<object[]> CandleMakerTestDataProp
        {
            get
            {
                yield return new object[] { TimeScales, Boards[0] };
                yield return new object[] { TimeScales, Boards[1] };
            }
        }

        #endregion

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="output"></param>
        public CandleMakerTest(ITestOutputHelper output)
        {
            Output = output;

            // ���K�[�쐬
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>().AddNLog();

            Logger = factory.CreateLogger<CandleMakerTest>();
        }

        [Theory(DisplayName = "CandleMaker�쐬")]
        [MemberData(nameof(CandleMakerTestDataProp))]
        [Trait("Category", "CandleMaker�̍쐬")]
        public void Test1(List<MTimeScale> timeScales, MBoard board)
        {
            // InMemoryDB���g���I�v�V�������쐬
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test1")
                .Options;

            CandleMaker data = null;
            using (var dbContext = new ApplicationDbContext(options)) // InMemoryDB���g���I�v�V�����ݒ��n��
            {
                // �C���X�^���X�쐬
                data = CandleMaker.MakeGeneration(Logger, dbContext, timeScales, board);
            }

            // ���O�̊m�F
            data.TimeScale.DisplayName.Is("1��");

            // ���̎��ԑ��f�[�^�̏ꍇ�A��Ԑe�̎q�i2��ځj��2���邱��
            data.Children.Count.Is(2);

            // �񐔂������Ĉ�ԑ傫���b���̎q���e����H��邱��
            // �\�������������ݒ肳��Ă��邱��
            data.Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].TimeScale.DisplayName.Is("1��");
        }

        [Fact(DisplayName = "CandleMaker�e�̃e�X�g")]
        [Trait("Category", "CandleMaker�̓���")]
        public void Test2()
        {
            // InMemoryDB���g���I�v�V�������쐬
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test2")
                .Options;

            CandleMaker data = null;
            using (var dbContext = new ApplicationDbContext(options)) // InMemoryDB���g���I�v�V�����ݒ��n��
            {
                // �C���X�^���X�쐬
                data = CandleMaker.MakeGeneration(Logger, dbContext, TimeScales, Boards[0]);

                // Ticker����͂��Ă���
                data.Update(Tickers1m[0]); // "08/18/2018 07:22:16" 800000
                // �ŏ���Ticker�����͂��ꂽ��A���[�\�N���쐬�����
                data.CurrentCandle.IsNotNull();
                data.CurrentCandle.Begin.Is(800000);
                data.CurrentCandle.End.Is(800000);
                data.CurrentCandle.Min.Is(800000);
                data.CurrentCandle.Max.Is(800000);
                // 1�����Ȃ̂ŁA�J�n�ƏI�����Ԃ�1�����Ƃɐ؂�̂�
                data.CurrentCandle.BeginTime.Is(DateTime.Parse("08/18/2018 07:22:00"));
                data.CurrentCandle.EndTime.Is(DateTime.Parse("08/18/2018 07:23:00"));

                // ���[�\�N�X�V�m�F:���l�X�V
                data.Update(Tickers1m[1]);  // "08/18/2018 07:22:18" 802000
                data.CurrentCandle.Begin.Is(800000);
                data.CurrentCandle.End.Is(802000);
                data.CurrentCandle.Min.Is(800000);
                data.CurrentCandle.Max.Is(802000);

                // ���[�\�N�X�V�m�F:���l�X�V
                // 1���o�߂��Ă��Ȃ��ꍇ�A���[�\�N���X�g�ɉ����Ȃ��i���E�l�e�X�g�j
                data.Update(Tickers1m[2]);  // "08/18/2018 07:22:59" 799000
                data.CurrentCandle.Begin.Is(800000);
                data.CurrentCandle.End.Is(799000);
                data.CurrentCandle.Min.Is(799000);
                data.CurrentCandle.Max.Is(802000);
                data.CandleList.Count.Is(0);

                // 1���o�߂����烍�[�\�N���X�g�ɉ�����
                // 1���o�߂������Ticker���i�̓��[�\�N���X�g�̃��[�\�N�ɉe�����Ȃ�
                // 1���o�߂������Ticker���i�͐V�������[�\�N�ɔ��f����
                data.Update(Tickers1m[3]); // "08/18/2018 07:23:00" 803000
                data.CandleList.Count.Is(1);
                data.CandleList[0].Begin.Is(800000);
                data.CandleList[0].End.Is(799000);
                data.CandleList[0].Min.Is(799000);
                data.CandleList[0].Max.Is(802000);
                data.CurrentCandle.Begin.Is(803000);
                data.CurrentCandle.End.Is(803000);
                data.CurrentCandle.Min.Is(803000);
                data.CurrentCandle.Max.Is(803000);

                // 5���o�߂����烍�[�\�N5�{�ɂȂ��Ă���
                // �Ԃ̃��[�\�N��Min = Max�ɂȂ��Ă���
                data.Update(Tickers1m[4]); // "08/18/2018 07:27:00" 805000
                data.CandleList.Count.Is(5);
                data.CandleList[3].Begin.Is(803000);
                data.CandleList[3].End.Is(803000);
                data.CandleList[3].Min.Is(803000);
                data.CandleList[3].Max.Is(803000);
                data.CandleList[4].Begin.Is(803000);
                data.CandleList[4].End.Is(803000);
                data.CandleList[4].Min.Is(803000);
                data.CandleList[4].Max.Is(803000);
                data.CurrentCandle.Begin.Is(805000);
                data.CurrentCandle.End.Is(805000);
                data.CurrentCandle.Min.Is(805000);
                data.CurrentCandle.Max.Is(805000);

                // 101���o�߂����烁�����̍ŏ��̃��[�\�N���폜����
                data.Update(Tickers1m[5]); // "08/18/2018 09:08:00" 806000
                data.CandleList.Count.Is(SystemConstants.MaxCandle);
                data.CandleList[0].BeginTime.Is(DateTime.Parse("08/18/2018 07:23:00"));
            }
        }

        [Fact(DisplayName = "CandleMaker�q�̃e�X�g")]
        [Trait("Category", "CandleMaker�̓���")]
        public void Test3()
        {
            // InMemoryDB���g���I�v�V�������쐬
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test3")
                .Options;

            CandleMaker data = null;
            using (var dbContext = new ApplicationDbContext(options)) // InMemoryDB���g���I�v�V�����ݒ��n��
            {
                // �C���X�^���X�쐬
                data = CandleMaker.MakeGeneration(Logger, dbContext, TimeScales, Boards[0]);

                // �e��Ticker����͂��Ă���
                data.Update(Tickers5m[0]);  // "08/18/2018 07:22:16" 800000
                // �ŏ���Ticker�����͂��ꂽ��A���[�\�N���쐬�����
                // 5�����Ȃ̂ŁA�J�n�ƏI�����Ԃ�5�����Ƃɐ؂�̂�
                data.Children[0].CurrentCandle.BeginTime.Is(DateTime.Parse("08/18/2018 07:20:00"));
                data.Children[0].CurrentCandle.EndTime.Is(DateTime.Parse("08/18/2018 07:25:00"));
                data.Children[0].CurrentCandle.Begin.Is(800000);
                data.Children[0].CurrentCandle.End.Is(800000);
                data.Children[0].CurrentCandle.Min.Is(800000);
                data.Children[0].CurrentCandle.Max.Is(800000);

                // 1���o�߂����烍�[�\�N���X�V�����
                data.Update(Tickers5m[1]);  // "08/18/2018 07:23:00" 802000
                data.Children[0].CurrentCandle.Begin.Is(800000);
                data.Children[0].CurrentCandle.End.Is(802000);
                data.Children[0].CurrentCandle.Min.Is(800000);
                data.Children[0].CurrentCandle.Max.Is(802000);

                // 5���o�߂��Ă��Ȃ��ꍇ�A���[�\�N���X�g�ɉ����Ȃ��i���E�l�e�X�g�j
                // ���l�X�V
                data.Update(Tickers5m[2]);  // "08/18/2018 07:24:59" 799000
                data.Children[0].CurrentCandle.Begin.Is(800000);
                data.Children[0].CurrentCandle.End.Is(799000);
                data.Children[0].CurrentCandle.Min.Is(799000);
                data.Children[0].CurrentCandle.Max.Is(802000);
                data.Children[0].CandleList.Count.Is(0);

                // 5���o�߂����烍�[�\�N���X�g�ɉ�����
                // 5���o�߂������Ticker���i�̓��[�\�N���X�g�̃��[�\�N�ɉe�����Ȃ�
                // 5���o�߂������Ticker���i�͐V�������[�\�N�ɔ��f����
                data.Update(Tickers5m[3]);  // "08/18/2018 07:25:00" 803000
                data.Children[0].CandleList[0].Begin.Is(800000);
                data.Children[0].CandleList[0].End.Is(799000);
                data.Children[0].CandleList[0].Min.Is(799000);
                data.Children[0].CandleList[0].Max.Is(802000);
                data.Children[0].CurrentCandle.Begin.Is(803000);
                data.Children[0].CurrentCandle.End.Is(803000);
                data.Children[0].CurrentCandle.Min.Is(803000);
                data.Children[0].CurrentCandle.Max.Is(803000);
                data.Children[0].CandleList.Count.Is(1);

                // 15���o�߂����烍�[�\�N3�{�ɂȂ��Ă���
                // �Ԃ̃��[�\�N��Min = Max�ɂȂ��Ă���
                data.Update(Tickers5m[4]);  // "08/18/2018 07:35:00" 805000
                data.Children[0].CandleList.Count.Is(3);
                data.Children[0].CandleList[1].Begin.Is(803000);
                data.Children[0].CandleList[1].End.Is(803000);
                data.Children[0].CandleList[1].Min.Is(803000);
                data.Children[0].CandleList[1].Max.Is(803000);
                data.Children[0].CandleList[2].Begin.Is(803000);
                data.Children[0].CandleList[2].End.Is(803000);
                data.Children[0].CandleList[2].Min.Is(803000);
                data.Children[0].CandleList[2].Max.Is(803000);
                data.Children[0].CurrentCandle.Begin.Is(805000);
                data.Children[0].CurrentCandle.End.Is(805000);
                data.Children[0].CurrentCandle.Min.Is(805000);
                data.Children[0].CurrentCandle.Max.Is(805000);

                // 505���o�߂����烁�����̍ŏ��̃��[�\�N���폜����
                data.Update(Tickers5m[5]);  // "08/18/2018 09:08:00" 806000
                data.Children[0].CandleList.Count.Is(SystemConstants.MaxCandle);
                data.Children[0].CandleList[0].BeginTime.Is(DateTime.Parse("08/18/2018 07:25:00"));
            }
        }

        #region xUnit�̊�{�@�\
        [Fact(DisplayName = "xUnit��NLog���g�p����")]
        [Trait("Category", "xUnit�̊�{�@�\")]
        public void HelloLogger()
        {
            Logger.LogTrace("World Trace");
            Logger.LogDebug("World Debug");
            Logger.LogWarning("World Warn");
            Logger.LogError("����World Error����");
        }

        [Fact(DisplayName = "���߂Ă�InMemoryDatabase")]
        [Trait("Category", "xUnit�̊�{�@�\")]
        public void AddDatabase()
        {
            // InMemoryDB���g���I�v�V�������쐬
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // �����f�[�^��ǉ��Busing �X�e�[�g�����g�ň�x DB �Ƃ̐ڑ���؂�B
            using (var context = new ApplicationDbContext(options)) // InMemoryDB���g���I�v�V�����ݒ��n��
            {
                foreach (var item in Currencies)
                {
                    context.MCurrencies.Add(item);
                }

                context.SaveChanges(SystemConstants.SystemName);
            }
            // �ؒf

            // �Đڑ�����InMemoryDB����l�����o��
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