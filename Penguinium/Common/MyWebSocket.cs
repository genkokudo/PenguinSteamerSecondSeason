using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;

namespace Penguinium.Common
{
    /// <summary>
    /// 1つのWebSocket
    /// </summary>
    public class MyWebSocket
    {
        /// <summary>
        /// 再接続待機時間（秒）
        /// </summary>
        const int ReconnectSeconds = 20;
        /// <summary>
        /// メッセージ受信イベント名
        /// </summary>
        const string ChannelMessage = "channelMessage";
        /// <summary>
        /// エンドポイント（wss://なんとか）
        /// </summary>
        public string EndPoint { get; }
        /// <summary>
        /// チャンネル名
        /// </summary>
        public string ChannelName { get; }
        /// <summary>
        /// 必要ならLoggerを設定
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// 1つのWebSocket
        /// </summary>
        /// <param name="endPoint">エンドポイント（wss://なんとか）</param>
        /// <param name="channelName">チャンネル名</param>
        /// <param name="logger">ログ出力する場合は設定する</param>
        public MyWebSocket(string endPoint, string channelName, ILogger logger = null)
        {
            EndPoint = endPoint;
            ChannelName = channelName;
            Logger = logger;
        }

        JsonRpc Rpc = null;
        ClientWebSocket ws = null;
        bool isWebSocketConnected = false;
        /// <summary>
        /// 受信を開始する
        /// 繋がるまでリトライする
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            do
            {
                try
                {
                    Logger.LogWarning($"接続を試みます {ToString()}");
                    ws = new ClientWebSocket();
                    await ws.ConnectAsync(new Uri(EndPoint), CancellationToken.None);

                    await ConnectRpcAsync(ws);
                    isWebSocketConnected = true;
                }
                catch (Exception)
                {
                    Logger.LogWarning($"接続できません、{ReconnectSeconds}秒後に再接続します {ToString()}");
                    await Task.Delay(ReconnectSeconds * 1000);
                }
            } while (!isWebSocketConnected);
        }

        private async Task ConnectRpcAsync(ClientWebSocket ws)
        {
            Rpc = new JsonRpc(new WebSocketMessageHandler(ws));

            Rpc.Disconnected += (s, e) =>
            {
                // 切断されたときの処理
                if (Logger != null)
                {
                    Logger.LogWarning($"切断されました {ToString()}");
                    isWebSocketConnected = false;
                    var t = RunAsync();
                }
            };
            Rpc.AddLocalRpcMethod(ChannelMessage, new Action<JToken, CancellationToken>((@params, cancellationToken) =>
            {
                // 受信したときの処理
                var p = @params as dynamic;
                Console.WriteLine($"{p.channel}: {p.message}");
            }));

            Rpc.StartListening();

            await Rpc.InvokeWithParameterObjectAsync<object>("subscribe", new { channel = ChannelName });
        }

        public override string ToString()
        {
            return $"EndPoint:{EndPoint} ChannelName:{ChannelName}";
        }
    }

}