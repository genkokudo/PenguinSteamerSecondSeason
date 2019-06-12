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
        // データ
        List<MTimeScale> timeScales = new List<MTimeScale>
            {
                new MTimeScale() { DisplayName = "1分", SecondsValue = 60 },
                new MTimeScale() { DisplayName = "3分", SecondsValue = 180 },
                new MTimeScale() { DisplayName = "5分", SecondsValue = 300 },
                new MTimeScale() { DisplayName = "10分", SecondsValue = 600 },
                new MTimeScale() { DisplayName = "30分", SecondsValue = 1800 },
                new MTimeScale() { DisplayName = "1時間", SecondsValue = 3600 },
                new MTimeScale() { DisplayName = "2時間", SecondsValue = 7200 },
                new MTimeScale() { DisplayName = "4時間", SecondsValue = 14400 },
                new MTimeScale() { DisplayName = "12時間", SecondsValue = 43200 },
                new MTimeScale() { DisplayName = "1日", SecondsValue = 86400 }
            };

        [Fact(DisplayName = "CandleMaker作成")]
        public void Test1()
        {
            // インスタンス作成
            var data = CandleMaker.MakeGeneration(null, timeScales);

            data.DisplayName.Is("1分");
        }

        [Fact(DisplayName = "子が2つあること")]
        public void Test2()
        {
            // インスタンス作成
            var data = CandleMaker.MakeGeneration(null, timeScales);

            data.Children.Count.Is(2);
        }

        [Fact(DisplayName = "約数があって一番大きい秒数の子が親から辿れること")]
        public void Test3()
        {
            // インスタンス作成
            var data = CandleMaker.MakeGeneration(null, timeScales);

            data.Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].DisplayName.Is("1日");
        }


    }
}
