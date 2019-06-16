using System;
using System.Collections.Generic;
using System.Text;

namespace PenguinSteamerSecondSeason
{
    /// <summary>
    /// システム設定
    /// </summary>
    public class SystemConstants
    {
        /// <summary>
        /// 環境を示す環境変数名のプレフィクス
        /// </summary>
        public const string PrefixEnv = "DOTNETCORE_";
        /// <summary>
        /// DBパスワード環境変数名
        /// </summary>
        public const string DbPasswordEnv = "DATABASE_PASSWORD";
        /// <summary>
        /// 環境を示す環境変数名のプレフィクス
        /// </summary>
        public const string EnxEnv = PrefixEnv + "ENVIRONMENT";

        /// <summary>
        /// 現在のDB接続設定
        /// 詳細はappsettings.json
        /// </summary>
        public const string Connection = "DefaultConnection";

        // 環境
        /// <summary>
        /// 環境：開発系
        /// </summary>
        public const string EnvDevelopment = "Development";
        /// <summary>
        /// 環境：本番系
        /// </summary>
        public const string EnvProduction = "Production";

        /// <summary>
        /// システム名
        /// データベース登録時に使用する
        /// </summary>
        public const string SystemName = "PenguinSteamer";

        /// <summary>
        /// MBoardの名前のプレフィクス：bitFlyer
        /// </summary>
        public const string BoardPrefixBitflyer = "BITFLYER";

        /// <summary>
        /// 各ローソク作成時の最大本数
        /// 取り敢えず固定値
        /// </summary>
        public const int MaxCandle = 200;

        /// <summary>
        /// 起動してからローソク初回保存時
        /// 同じ板のデータを全削除
        /// </summary>
        public const bool IsFirstDeleteCandle = true;

    }
}
