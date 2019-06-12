using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinSteamerSecondSeason.Common
{
    /// <summary>
    /// 文字列を1つ渡すイベント
    /// </summary>
    public class TextEventArgs : EventArgs
    {
        public string Text { get; }

        public TextEventArgs(string text)
        {
            Text = text;
        }
    }
}
