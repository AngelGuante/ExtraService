using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using System.Threading.Tasks;
using static ExtraService.MessageReceived;

namespace ExtraService{
    public class wsConnection{
    private static ClientWebSocket _client = new ClientWebSocket();

        public static async Task ConnectToServerAsync()
        {
            try
            {
                await _client.ConnectAsync(new Uri($"ws://localhost:5000/ws"), CancellationToken.None);
                // await _client.ConnectAsync(new Uri($"ws://monicawebsocketserver.azurewebsites.net/ws"), CancellationToken.None);

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
                });
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

        public static async void EndConnectionAsync() =>
            await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "close", CancellationToken.None);
    }
}