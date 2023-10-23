using Backend.Managers;
using Backend.Models.Users;
using Bragi.Extensions;
using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class Auth : ControllerBase
{
  private readonly HttpContext _context;
  private readonly SessionManager _sessionManager;
  private readonly UserManager _userManager;
  public Auth(UserManager userManager, IHttpContextAccessor context, SessionManager sessionManager)
  {
    _userManager = userManager;
    _context = context.HttpContext!;
    _sessionManager = sessionManager;
  }

  public class AuthModel
  {
    public string Login { get; set; }
    public string Password { get; set; }
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] AuthModel login)
  {

    UserSession session = UserSession.FromContext(_context);
    if (session.Logged)
    {
      return BadRequest(this.GetStatusError(HttpStatusCode.BadRequest, "auth", "You are already logged in"));
    }

    User? user = await _userManager.GetUserByLogin(login.Login, true);
    if (user == null)
    {
      return BadRequest(this.GetStatusError(HttpStatusCode.BadRequest, "auth", "Invalid login or password"));
    }

    if (!user.CheckPassword(login.Password))
    {
      return BadRequest(this.GetStatusError(HttpStatusCode.BadRequest, "auth", "Invalid login or password"));
    }

    session.User = user;
    
    

    Dictionary<string, string> data = new Dictionary<string, string>();
    data.Add("token", session.SessionId);
    return Ok(data);

  }
}

