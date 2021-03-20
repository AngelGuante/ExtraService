using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using System.Threading.Tasks;
using static ExtraService.MessageReceived;
using static ExtraService.Utils.RequestsHTTP;

namespace ExtraService
{
    public class wsConnection
    {
        private static int totalReconections = 0;
        // private static string _wsServerUrl = "localhost:5000";
        private static string _wsServerUrl = "monicawebsocketserver.azurewebsites.net";
        private static ClientWebSocket _client = new ClientWebSocket();
        public static async Task ConnectToServerAsync()
        {
            try
            {
                await GET($"https://{_wsServerUrl}/API/SendDataClient/RemoveClient");
                await Task.Delay(2000);
                await _client.ConnectAsync(new Uri($"ws://{_wsServerUrl}/ws"), CancellationToken.None);
                await Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        WebSocketReceiveResult result;
                        ArraySegment<byte> message = new ArraySegment<byte>(new byte[4096]);

                        do
                        {
                            result = await _client.ReceiveAsync(message, CancellationToken.None);
                            byte[] messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                            string serialisedMessage = Encoding.UTF8.GetString(messageBytes);

                            Message(serialisedMessage);
                        } while (!result.EndOfMessage);
                    }
                }); new ClientWebSocket();
            }
            catch (Exception e)
            {
                Message($"Error: {e.Message}");
            }
        }

        public static async void SendMessageAsync(string message)
        {
            try
            {
                string serialisedMessage = message;
                var byteMessage = Encoding.UTF8.GetBytes(serialisedMessage);
                var segmnet = new ArraySegment<byte>(byteMessage, 0, byteMessage.Length);

                await _client.SendAsync(segmnet, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                Message($"Error: {e.Message}");
            }
        }

        public static async Task CounterAsync()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(2000);
                    if (_client.State != WebSocketState.Open)
                    {
                        Console.WriteLine($"\n-------------------------------------------------------------");
                        Console.WriteLine($"--RECONECTANDO- Total de reconecciones: {++totalReconections}--");
                        Console.WriteLine($"--------------------------------------------------------------\n");

                        _client = new ClientWebSocket();
                        await GET($"https://{_wsServerUrl}/API/SendDataClient/RemoveClient");
                        // await GET($"https://localhost:5001/API/SendDataClient/RemoveClient");
                        await Task.Delay(2000);

                        await ConnectToServerAsync();
                        Console.WriteLine($"\n-------------");
                        Console.WriteLine($"--RECONECTADO--");
                        Console.WriteLine($"-------------\n");
                        await Task.Delay(2000);
                    }
                }
            });
        }
    }
}