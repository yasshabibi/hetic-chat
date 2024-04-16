using Backend.Enums;

namespace Backend.Models.Users;

public class User
{
  public long ID { get; init; }
  public string Username { get; init; }
  public string UsernameSafe => Username.ToLower().Replace(" ", "_").Trim();
  public string Email { get; init; }
  public string PasswordHash { get; init; }
}
