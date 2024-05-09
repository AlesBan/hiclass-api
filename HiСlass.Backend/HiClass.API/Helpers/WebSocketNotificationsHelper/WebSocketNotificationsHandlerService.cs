using System.Net.WebSockets;
using System.Text;

namespace HiClass.API.Helpers.WebSocketNotificationsHelper;

public static class WebSocketNotificationsHandlerService
{
    public static async Task Send(HttpContext context, WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];

        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
            Console.WriteLine($"Client says: {message}");

            await webSocket.SendAsync(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Server says: {DateTime.UtcNow:f}")),
                result.MessageType, result.EndOfMessage, CancellationToken.None);

            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}