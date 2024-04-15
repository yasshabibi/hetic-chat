using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class WebSocketController : ControllerBase
{
    // Stockage de tous les sockets clients connectés
    private static readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

    [HttpGet("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            string socketId = Guid.NewGuid().ToString();
            _sockets.TryAdd(socketId, webSocket);

            try
            {
                await ReceiveMessage(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string userMessage = Encoding.UTF8.GetString(buffer.ToArray(), 0, buffer.Length);
                        await RouteMessage(userMessage, webSocket);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        WebSocket dummy;
                        _sockets.TryRemove(socketId, out dummy);
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closed", CancellationToken.None);
                    }
                });
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                WebSocket dummy;
                _sockets.TryRemove(socketId, out dummy);
                // Attempt to close the socket if it hasn't been closed already
                if (webSocket.State != WebSocketState.Closed)
                    webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal server error", CancellationToken.None).Wait();
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 400;
        }
    }

    private async Task ReceiveMessage(WebSocket socket, Action<ValueWebSocketReceiveResult, Memory<byte>> handleMessage)
    {
        Memory<byte> buffer = new byte[1024 * 4];

        while (socket.State == WebSocketState.Open)
        {
            ValueWebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            handleMessage(result, buffer);
        }
    }

    private async Task RouteMessage(string message, WebSocket incomingSocket)
    {
        // Here, you can decide what to do with incoming messages
        // For example, broadcasting messages to all connected clients
        foreach (var socket in _sockets)
        {
            if (socket.Value.State == WebSocketState.Open && socket.Value != incomingSocket)
            {
                var serverMsg = Encoding.UTF8.GetBytes("Received: " + message);
                await socket.Value.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    public static async Task BroadcastMessage(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _sockets)
        {
            if (socket.Value.State == WebSocketState.Open)
            {
                await socket.Value.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
