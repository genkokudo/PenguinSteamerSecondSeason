using System;

namespace Client
{
    public class Client
    {
        public static void Main()
        {
            string ipOrHost = "127.0.0.1";
            int port = 2001;
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            string sendMsg = "test";

            // 接続要求
            System.Net.Sockets.TcpClient tcp =
              new System.Net.Sockets.TcpClient(ipOrHost, port);
            Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
                ((System.Net.IPEndPoint)tcp.Client.RemoteEndPoint).Address,
                ((System.Net.IPEndPoint)tcp.Client.RemoteEndPoint).Port,
                ((System.Net.IPEndPoint)tcp.Client.LocalEndPoint).Address,
                ((System.Net.IPEndPoint)tcp.Client.LocalEndPoint).Port);

            // 接続したら送受信に使うネットワークストリームを取得する
            System.Net.Sockets.NetworkStream ns = tcp.GetStream();
            while (!string.IsNullOrEmpty(sendMsg))
            {
                Console.WriteLine("入力してください");
                sendMsg = Console.ReadLine();
                Console.WriteLine("送信:" + sendMsg);

                if (string.IsNullOrEmpty(sendMsg))
                {
                    return;
                }

                // タイムアウト時間を指定(ms)
                ns.ReadTimeout = 10000;
                ns.WriteTimeout = 10000;

                // 入力文字をバイト配列にして送信（ストリームに書き込み）
                byte[] sendBytes = enc.GetBytes(sendMsg + '\n');
                ns.Write(sendBytes, 0, sendBytes.Length);

                // 送り終わったら今度は受信する番
                // 受信用のメモリストリームを作成
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                byte[] resBytes = new byte[256];
                int resSize;
                do
                {
                    // ネットワークストリームを読んで
                    resSize = ns.Read(resBytes, 0, resBytes.Length);

                    if (resSize == 0)
                    {
                        Console.WriteLine("サーバーが切断しました。");
                        break;
                    }

                    // 受信用のメモリストリームに入れる
                    ms.Write(resBytes, 0, resSize);

                } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');    // データが無くなるか、改行文字が来たら終了

                // 受信用のメモリストリームから文字列を取り出す
                string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                // 受信用のメモリストリームを捨てちゃう
                ms.Close();

                resMsg = resMsg.TrimEnd('\n');
                Console.WriteLine("受信:" + resMsg);

            }
            ns.Close();
            tcp.Close();
            Console.WriteLine("切断しました。");

            Console.ReadLine();
        }
    }
}
