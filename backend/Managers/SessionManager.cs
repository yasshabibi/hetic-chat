﻿using Backend.Models.Users;
using Backend.Utils;

namespace Backend.Managers;

public class SessionManager
{
  public IReadOnlyDictionary<string, UserSession> Sessions => _sessions;

  private static readonly Dictionary<string, UserSession> _sessions = new Dictionary<string, UserSession>();

  public SessionManager()
  {
    Console.WriteLine("SessionManager initialized");
  }
  
  public UserSession CreateSession(HttpContext context)
  {
    bool found = false;
    string sessionId = string.Empty;
    UserSession? session = null;
    while (!found)
    {
      sessionId = StringUtils.RandomString(32, numericals:true, caseVariant:true);
      if (Sessions.ContainsKey(sessionId)) 
        continue;
      
      session = new UserSession(sessionId, context.Connection.RemoteIpAddress);
      _sessions.Add(sessionId, session);
      found = true;
    }

    return session!;
  }

  public bool CheckSession(string sessionId)
  {
    if (!Sessions.ContainsKey(sessionId)) 
      return false;
    UserSession session = Sessions[sessionId];
    if (session.LastActivity.AddMinutes(30) >= DateTime.UtcNow) 
      return true;
    _sessions.Remove(sessionId);
    return false;

  }

  public UserSession IdentifyUser(HttpContext context, bool updateSessionTime = false)
  {
    context.Request.Headers.TryGetValue("session", out var header);
    string sessionId = header.FirstOrDefault() ?? string.Empty;
    UserSession session;
    
    if (string.IsNullOrEmpty(sessionId) || !CheckSession(sessionId))
    {
      session = CreateSession(context);
      context.Response.Cookies.Append("session", session.SessionId,
        new CookieOptions { Expires = DateTime.UtcNow.AddDays(1) });
      return session;
    }
    
    session = Sessions[sessionId];
    if (updateSessionTime)
      session.LastActivity = DateTime.Now;
    return session;
    
  }
}
