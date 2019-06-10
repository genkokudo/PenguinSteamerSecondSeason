using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinSteamerSecondSeason.Common
{
    /// <summary>
    /// Tickerからローソク足データを作成する
    /// 取引所と時間足ごとにこのクラスオブジェクトを作成する
    /// このクラスは親子関係を持ち、複数の子を持つ場合もある
    /// 1分足が1番親になる
    /// 親が更新したら、子に更新が伝搬する
    /// </summary>
    class CandleMaker
    {
    }
}
