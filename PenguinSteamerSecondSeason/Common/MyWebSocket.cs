using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using PenguinSteamerSecondSeason.Models.RawData;
using Newtonsoft.Json;

namespace PenguinSteamerSecondSeason.Common
{
    /// <summary>
    /// 1つのWebSocket
    /// Ticker以外にも
    /// </summary>
    public class MyWebSocket
    {
        /// <summary>
        /// 再接続待機時間（秒）
        /// </summary>
        const int ReconnectSeconds = 20;
        /// <summary>
        /// メッセージ受信イベント名
        /// DBに入れなきゃいかんのちゃう？
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
        /// メッセージを受信したときのイベント
        /// </summary>
        public event EventHandler GetMessage;

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

        /// <summary>
        /// RPCに接続する
        /// </summary>
        /// <param name="ws">WebSocket</param>
        /// <returns></returns>
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

                OnGetMessage(p.message.ToString());
            }));

            Rpc.StartListening();

            await Rpc.InvokeWithParameterObjectAsync<object>("subscribe", new { channel = ChannelName });
        }

        public override string ToString()
        {
            return $"EndPoint:{EndPoint} ChannelName:{ChannelName}";
        }

        /// <summary>
        /// メッセージを受信したときに呼び出すイベント
        /// </summary>
        /// <param name="e">message</param>
        protected virtual void OnGetMessage(EventArgs e)
        {
            // nullじゃなければ呼び出す
            GetMessage?.Invoke(this, e);
        }
    }
}