using System.Net;
using System.Net.WebSockets;

namespace Backend.Models.Users;

public class UserSession
{
    public User? User { get; set; }
    public string SessionId { get; }
    public bool Logged => User != null;
    public IPAddress? IpAddress { get; init; }
    public DateTime LastActivity { get; set; } = DateTime.UtcNow;
    public WebSocket? WebSocket { get; set; }

    public UserSession(string sessionId, IPAddress? ipAddress)
    {
        SessionId = sessionId;
        IpAddress = ipAddress;
    }

    public static UserSession FromContext(HttpContext context)
    {
        return (context.Items["session"] as UserSession)!;
    }
}
