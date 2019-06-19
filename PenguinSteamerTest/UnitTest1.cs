using ChainingAssertion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PenguinSteamerSecondSeason;
using PenguinSteamerSecondSeason.Common;
using PenguinSteamerSecondSeason.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace PenguinSteamerTest
{
    // TODO: InMemory
    // TODO: ���O

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
        /// </summary>
        private static readonly List<MBoard> Boards = new List<MBoard>
            {
                new MBoard() { Id = 11, Name = "BITFLYER_BTC_JPY", DisplayName = "bitFlyer����BTC" },
                new MBoard() { Id = 12, Name = "BITFLYER_FX_BTC_JPY", DisplayName = "bitFlyerFX" },
                new MBoard() { Id = 13, Name = "BITFLYER_ETH_BTC", DisplayName = "bitFlyer����ETH" },
                new MBoard() { Id = 14, Name = "BITFLYER_BCH_BTC", DisplayName = "bitFlyer����BCH" }
            };
        #endregion

        #region �e�X�g�f�[�^
        public static IEnumerable<object[]> CandleMakerTestDataProp
        {
            get
            {
                yield return new object[] { null, TimeScales, Boards[0] };
                yield return new object[] { null, TimeScales, Boards[1] };
            }
        }
        #endregion

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
        [Trait("Category", "Construct")]
        public void Test1(ApplicationDbContext dbContext, List<MTimeScale> timeScales, MBoard board)
        {
            // �C���X�^���X�쐬
            var data = CandleMaker.MakeGeneration(Logger, dbContext, timeScales, board);

            // ���O�̊m�F
            data.DisplayName.Is("1��");

            // ���̎��ԑ��f�[�^�̏ꍇ�A��Ԑe�̎q��2���邱��
            data.Children.Count.Is(2);

            // �񐔂������Ĉ�ԑ傫���b���̎q���e����H��邱��
            // �\�������������ݒ肳��Ă��邱��
            data.Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].DisplayName.Is("1��");
        }

        [Fact(DisplayName = "xUnit��NLog���g�p����")]
        [Trait("Category", "Logging")]
        public void Hello()
        {
            Logger.LogTrace("World Trace");
            Logger.LogDebug("World Debug");
            Logger.LogWarning("World Warn");
            Logger.LogError("����World Error����");
        }

    }
}

///// <summary>
///// �ŐV��Ticker
///// �l�����͈̂�Ԑe�̂�
///// ���擾�̏ꍇ��null
///// </summary>
//public Ticker CurrentTicker { get; private set; }

///// <summary>
///// �ŐV�̃��[�\�N�i�쐬�r���̃��[�\�N�j
///// �����������[�\�N��DB��
///// </summary>
//public Candle CurrentCandle { get; private set; }

///// <summary>
///// ���܂ō�������[�\�N
///// </summary>
//public List<Candle> CandleList { get; }

///// <summary>
///// ���ԑ�
///// </summary>
//public MTimeScale TimeScale { get; }

///// <summary>
///// �ǂ̔�
///// </summary>
//public MBoard Board { get; }

//Update(Ticker ticker)
//        Ticker�Ń��[�\�N���X�V����
//        ���Ԃ��߂��Ă����烍�[�\�N�X�V�B
//        �傫�����Ԃ��߂��Ă����畡���{���[�\�N�X�V�B
        
//        ���Ԃ��߂��Ă�����q���X�V����

//        �ő�f�[�^���𒴂��Ă�����Â��f�[�^���폜���遦�f�[�^�������K�v
        
//        ��Candle�N���X
//        Ticker�ɂ���ă��[�\�N���X�V����
//        ���[�\�N�f�[�^�ɂ���ă��[�\�N���X�V����