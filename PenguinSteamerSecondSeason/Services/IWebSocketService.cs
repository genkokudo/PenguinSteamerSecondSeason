using Microsoft.Extensions.Logging;
using PenguinSteamerSecondSeason.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PenguinSteamerSecondSeason.Services
{
    /// <summary>
    /// 複数のWebSocketを管理する
    /// </summary>
    public interface IWebSocketService
    {
        /// <summary>
        /// インジェクション先から
        /// このメソッドを呼び出すことができる
        /// </summary>
        /// <returns></returns>
        MyWebSocket Add(string endPoint, string channelName);
        Task StartAllAsync();
    }

    /// <summary>
    /// 複数のWebSocketを管理する
    /// 役割としては微妙だが、Penguiniumのクラスに何かあればここで何とかするということで。
    /// </summary>
    public class WebSocketService : IWebSocketService
    {
        /// <summary>
        /// 使用しているWebSocket
        /// </summary>
        private List<MyWebSocket> MyWebSockets { get; }
        ILogger<WebSocketService> Logger { get; }
        public WebSocketService(ILogger<WebSocketService> logger)
        {
            MyWebSockets = new List<MyWebSocket>();
            Logger = logger;
        }

        /// <summary>
        /// 扱うWebSocket追加
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public MyWebSocket Add(string endPoint, string channelName)
        {
            var myWebSocket = new MyWebSocket(endPoint, channelName, Logger);
            Logger.LogInformation($"WebSocket追加:{myWebSocket.ToString()}");
            MyWebSockets.Add(myWebSocket);
            return myWebSocket;
        }

        /// <summary>
        /// 全て開始
        /// </summary>
        /// <returns></returns>
        public async Task StartAllAsync()
        {
            // WebSocket全部繋ぎます
            var list = new List<Task>();
            foreach (var item in MyWebSockets)
            {
                Logger.LogInformation($"WebSocket接続:{item.ToString()}");
                list.Add(item.RunAsync());
            }
            await Task.WhenAll(list);
        }
    }

}
