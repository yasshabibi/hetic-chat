using Backend.Enums;

namespace Backend.Models.Users;

public class User
{
  public long ID { get; init; }
  public string LoginName { get; init; }
  public string UsernameSafe => LoginName.ToLower().Replace(" ", "_").Trim();
  public string DisplayName { get; set; }
  public string PasswordHash { get; init; }
  public UserRank Rank { get; init; }
  
  public bool ShowPassword { get; init; }
  
  public bool CheckPassword(string password)
  {
    if (!ShowPassword)
      throw new Exception("Model does not include password, can't check using this model");
    return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
  }
}
