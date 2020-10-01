using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebsocketGracefulShutdown31.Controllers
{
    public class TestController : Controller
    {
        /// <summary>
        /// Liveliness and readiness check async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task</returns>
        [HttpGetAttribute("/ws/test")]
        public async Task TestWsAsync(CancellationToken cancellationToken)
        {
            var webSocket = await this.HttpContext.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);

            var retryCounter = 0;
            while (true)
            {
                try
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(new byte[64000], 0, 64000), cancellationToken).ConfigureAwait(false);
                    retryCounter = 0;

                    var time = DateTime.Now;
                    Console.WriteLine($"[{time.Hour}:{time.Minute}:{time.Second}.{time.Millisecond}][Ws] Result=(type={result.MessageType}, size={result.Count}, eom={result.EndOfMessage}) CancellationToken={cancellationToken.IsCancellationRequested}, RequestAborted={this.HttpContext.RequestAborted.IsCancellationRequested}");

                    await Task.Delay(TimeSpan.FromMilliseconds(1000)).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    var time = DateTime.Now;
                    Console.WriteLine($"[{time.Hour}:{time.Minute}:{time.Second}.{time.Millisecond}][Ws] Retry={retryCounter}, Error={ex}");
                    if (retryCounter == 3)
                    {
                        break;
                    }

                    retryCounter++;
                }
            }
        }
    }
}
