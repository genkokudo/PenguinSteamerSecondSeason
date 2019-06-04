using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason.Services
{
    /// <summary>
    /// サービスを作ってみるテスト
    /// ここで外から呼ぶメソッドだけ定義していく
    /// 何で本体クラスとインタフェースが1対1なんだろ？
    /// 他のクラスにインジェクションする物だったらインタフェース作るって考えであってる？？
    /// </summary>
    public interface IMyService
    {
        /// <summary>
        /// インジェクション先から
        /// このメソッドを呼び出すことができる
        /// </summary>
        /// <returns></returns>
        void MyServiceMethod();
    }

    /// <summary>
    /// 本体
    /// </summary>
    public class MyService : IMyService
    {
        private readonly string _baseUrl;
        private readonly string _token;

        //private readonly ILogger<MyService> _logger;
        //public MyService(ILoggerFactory loggerFactory, IConfigurationRoot config)
        //{
        //    var baseUrl = config["SomeConfigItem:BaseUrl"];
        //    var token = config["SomeConfigItem:Token"];
        //    _baseUrl = baseUrl;
        //    _token = token;
        //    _logger = loggerFactory.CreateLogger<MyService>();
        //}
        public void MyServiceMethod()
        {
            Console.WriteLine("aaaa");
            //_logger.LogDebug(_baseUrl);
            //_logger.LogDebug(_token);
        }
    }

}
