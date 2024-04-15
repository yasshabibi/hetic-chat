using Backend.Enums;

namespace Backend.Models.Users;

public class User
{
  public long ID { get; init; }
  public string LoginName { get; init; }
  public string UsernameSafe => LoginName.ToLower().Replace(" ", "_").Trim();
  public string DisplayName { get; set; }
  public string PasswordHash { get; init; }
}
