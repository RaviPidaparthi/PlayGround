using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebsocketClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();

            await Task.WhenAll(RunAsync(10022), RunAsync(10031), RunAsync(10050)).ConfigureAwait(false);
        }

        static async Task RunAsync(int port)
        {
            try
            {
                var websocket = new ClientWebSocket();
                Console.WriteLine($"[{port}] Connecting...");
                await websocket.ConnectAsync(new Uri($"ws://localhost:{port}/ws/test"), CancellationToken.None).ConfigureAwait(false);
                Console.WriteLine($"[{port}] Connected to ...");
                while (true)
                {
                    var bytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
                    Console.WriteLine($"[{port}] Sending bytes");
                    await websocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Binary, true, CancellationToken.None).ConfigureAwait(false);
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{port}] Error. {ex}");
            }
        }
    }
}
