using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DemoTTCSCN.Services
{
    public class SendDataSocketService
    {
        public async Task<string> SendQuery(string query, string[] parameter = null)
        {
            var client = await SocketService.Instance.OpenSoketService();
            var cTs = new CancellationTokenSource();
            cTs.CancelAfter(TimeSpan.FromSeconds(120));
            try
            {
                var n = 0;
                string result = "";
                if (client.State == WebSocketState.Open)
                {
                    if (!string.IsNullOrEmpty(query))
                    {
                        if (parameter != null)
                        {
                            foreach (var item in parameter)
                            {
                                query = query + "$" + item;
                            }
                        }
                        ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(query));
                        await client.SendAsync(byteToSend, WebSocketMessageType.Text, true, cTs.Token);
                        var responseBuffer = new byte[1024];
                        var offset = 0;
                        var packet = 1024;
                        while (true)
                        {
                            ArraySegment<byte> byteRecieved = new ArraySegment<byte>(responseBuffer, offset, packet);
                            WebSocketReceiveResult response = await client.ReceiveAsync(byteRecieved, cTs.Token);
                            var reponseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                            result = !reponseMessage.Contains("This query is valid!") ? reponseMessage : null;
                            if (response.EndOfMessage) break;
                        }

                    }
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", cTs.Token);
                }
                return result;
            }
            catch (WebSocketException ex)
            {
                throw;
            }
        }

    }

    public class SocketService
    {
        private SocketService() { }
        private static volatile SocketService instance;
        static object key = new object();
        public static SocketService Instance
        {
            get
            {
                lock (key)
                {
                    if (instance == null)
                    {
                        instance = new SocketService();
                    }
                }
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        public async Task<ClientWebSocket> OpenSoketService()
        {
            ClientWebSocket client = new ClientWebSocket();
            Uri serviceUri = new Uri("ws://localhost:5000/send");
            var cTs = new CancellationTokenSource();
            cTs.CancelAfter(TimeSpan.FromSeconds(120));
            try
            {
                await client.ConnectAsync(serviceUri, cTs.Token);
            }
            catch (WebSocketException ex)
            {
                throw;
            }
            return client;
        }
    }
}