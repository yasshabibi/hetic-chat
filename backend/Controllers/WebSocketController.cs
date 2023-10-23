using System.Net.WebSockets;
using System.Text;
using Backend.Models;
using Backend.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Backend.Controllers
{
    [Route("/ws")]
    public class WebSocketController : ControllerBase
    {
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var session = UserSession.FromContext(HttpContext);

                if (!session.Logged)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return;
                }
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                session.WebSocket = webSocket;
                await HandleWebSocketMessages(session);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task HandleWebSocketMessages(UserSession session)
        {
            var webSocket = session.WebSocket;
            var buffer = new byte[1024 * 4];
            bool connected = true;
            while (connected)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        await HandleMessage(message, session);
                        break;
                    case WebSocketMessageType.Binary:
                        break;
                    case WebSocketMessageType.Close:
                        connected = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static async Task HandleMessage(string message, UserSession session)
        {
            var messageObject = JsonConvert.DeserializeObject<JObject>(message);
        }
    }
}