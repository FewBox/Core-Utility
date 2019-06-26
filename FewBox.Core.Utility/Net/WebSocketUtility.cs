using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FewBox.Core.Utility.Net
{
    public static class WebSocketUtility
    {
        public static bool IsCertificateNeedValidate { private get; set; }
        public static async Task<string> Post(string url, string token, IList<Header> headers)
        {
            using (var clientWebSocket = new ClientWebSocket())
            {
                RemoteCertificateValidationCallback callback = delegate { return true; };
                clientWebSocket.Options.RemoteCertificateValidationCallback = callback;
                clientWebSocket.Options.SetRequestHeader("Authorization", $"Bearer {token}");
                foreach (var header in headers)
                {
                    clientWebSocket.Options.SetRequestHeader(header.Key, header.Value);
                }
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    Uri uri;
                    if (url.ToLower().StartsWith("https"))
                    {
                        uri = new Uri(url.ToLower().Replace("https", "wss"));
                    }
                    else if (url.ToLower().StartsWith("http"))
                    {
                        uri = new Uri(url.ToLower().Replace("http", "ws"));
                    }
                    else
                    {
                        uri = new Uri(url.ToLower());
                    }
                    Task taskConnect = clientWebSocket.ConnectAsync(uri, cancellationTokenSource.Token);
                    await taskConnect;
                }
                byte[] buffer = new byte[1024*1];
                var segment = new ArraySegment<byte>(buffer);
                WebSocketReceiveResult webSocketReceiveResult;
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    webSocketReceiveResult = await clientWebSocket.ReceiveAsync(segment, cancellationTokenSource.Token);
                }
                await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                byte[] result = new byte[webSocketReceiveResult.Count];
                Array.Copy(buffer, result, webSocketReceiveResult.Count);
                return System.Text.Encoding.UTF8.GetString(result);
            }
        }
    }
}
