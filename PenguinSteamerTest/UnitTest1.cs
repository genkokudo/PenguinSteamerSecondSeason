using ChainingAssertion;
using PenguinSteamerSecondSeason.Common;
using PenguinSteamerSecondSeason.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace PenguinSteamerTest
{
    public class CandleMakerTest
    {
        // �f�[�^
        List<MTimeScale> timeScales = new List<MTimeScale>
            {
                new MTimeScale() { DisplayName = "1��", SecondsValue = 60 },
                new MTimeScale() { DisplayName = "3��", SecondsValue = 180 },
                new MTimeScale() { DisplayName = "5��", SecondsValue = 300 },
                new MTimeScale() { DisplayName = "10��", SecondsValue = 600 },
                new MTimeScale() { DisplayName = "30��", SecondsValue = 1800 },
                new MTimeScale() { DisplayName = "1����", SecondsValue = 3600 },
                new MTimeScale() { DisplayName = "2����", SecondsValue = 7200 },
                new MTimeScale() { DisplayName = "4����", SecondsValue = 14400 },
                new MTimeScale() { DisplayName = "12����", SecondsValue = 43200 },
                new MTimeScale() { DisplayName = "1��", SecondsValue = 86400 }
            };

        [Fact(DisplayName = "CandleMaker�쐬")]
        public void Test1()
        {
            // �C���X�^���X�쐬
            var data = CandleMaker.MakeGeneration(null, timeScales);

            data.DisplayName.Is("1��");
        }

        [Fact(DisplayName = "�q��2���邱��")]
        public void Test2()
        {
            // �C���X�^���X�쐬
            var data = CandleMaker.MakeGeneration(null, timeScales);

            data.Children.Count.Is(2);
        }

        [Fact(DisplayName = "�񐔂������Ĉ�ԑ傫���b���̎q���e����H��邱��")]
        public void Test3()
        {
            // �C���X�^���X�쐬
            var data = CandleMaker.MakeGeneration(null, timeScales);

            data.Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].DisplayName.Is("1��");
        }


    }
}
