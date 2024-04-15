namespace Backend.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Managers;
    using Models.Users;
    using System.ComponentModel.DataAnnotations;

    namespace Backend.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class UserController : ControllerBase
        {
            private readonly UserManager _userManager;
            private readonly SessionManager _sessionManager;


            public class UserRegistrationRequest
            {
                [Required]
                [StringLength(50, MinimumLength = 3)]
                public string Username { get; set; }

                [Required]
                [EmailAddress]
                [StringLength(100)]
                public string Email { get; set; }

                [Required]
                [StringLength(100, MinimumLength = 6)]
                public string Password { get; set; }
            }

            public class UserLoginRequest
            {
                [Required]
                [StringLength(50, MinimumLength = 3)]
                public string Username { get; set; }

                [Required]
                [StringLength(100, MinimumLength = 6)]
                public string Password { get; set; }
            }

            public UserController(UserManager userManager, SessionManager sessionManager)
            {
                _userManager = userManager;
                _sessionManager = sessionManager;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
            {
                // Assuming UserRegistrationRequest has Username, Email, and Password properties
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };
                var success = await _userManager.StoreUser(user);
                if (!success)
                    return BadRequest("Registration failed.");

                return Ok("User registered successfully.");
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
            {
                // Assuming UserLoginRequest has Username and Password properties
                var user = await _userManager.GetUser(request.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                    return Unauthorized("Invalid username or password.");

                // Creating a session or updating an existing session
                var session = _sessionManager.IdentifyUser(HttpContext, updateSessionTime: true);
                session.User = user;  // Associating the user with the session
                return Ok(new { SessionId = session.SessionId, Message = "Login successful" });
            }
        }
    }
}
