using Backend.Enums;
using Backend.Models.Users;
using Backend.Utils;

namespace Backend.Managers;

public class UserManager
{
  private readonly DatabaseManager _manager;
  public UserManager(DatabaseManager manager)
  {
    _manager = manager;
  }

  public async Task<User?> GetUser(string? username)
  {
    Dictionary<string, object>? fetch = await _manager.FetchOne(
      "SELECT id, username, email, avatar_md5, password_hash, rank, unix register FROM users where username_safe = @usafe;", 
      new Dictionary<string, object>
      {
        { "@usafe", username.ToLower().Replace(" ", "_").Trim() }
      }
    );

    if (fetch == null || fetch.Keys.Count == 0)
      return null;

    return new User
    {
      ID = fetch["id"] as long? ?? 0,
      Username = fetch["username"] as string ?? "",
      Email = fetch["email"] as string ?? "",
      AvatarMD5 = fetch["avatar_md5"] as string ?? "",
      PasswordHash = fetch["password_hash"] as string ?? "",
      Rank = (UserType) (fetch["rank"] as long? ?? 0),
      RegisterUnix = DateTimeOffset.FromUnixTimeSeconds(fetch["register"] as long? ?? 0)
    };
  }

  public bool VerifyPassword(User user, string? password)
  {
    return BCrypt.Net.BCrypt.Verify(password.CalculateMD5Hash(), user.PasswordHash);
  }
}
