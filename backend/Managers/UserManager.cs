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
            "SELECT user_id, username, email, password_hash FROM Users WHERE username = @username;",
            new Dictionary<string, object>
            {
                { "@username", username }
            }
        );

        if (fetch == null || fetch.Keys.Count == 0)
            return null;

        return new User
        {
            ID = fetch["user_id"] as int? ?? 0,
            Username = fetch["username"] as string ?? "",
            Email = fetch["email"] as string ?? "",
            PasswordHash = fetch["password_hash"] as string ?? "",
        };
    }

    public async Task<bool> StoreUser(User user)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@username", user.Username },
            { "@email", user.Email },
            { "@passwordHash", user.PasswordHash }
        };

        string query = @"
      INSERT INTO Users (username, email, password_hash)
      VALUES (@username, @email, @password_hash);
    ";

        try
        {
            await _manager.Execute(query, parameters);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteUser(int userId, int requesterId)
    {
        if (userId == requesterId)
        {
            string query = "DELETE FROM Users WHERE user_id = @userId;";
            var parameters = new Dictionary<string, object> { { "@userId", userId } };
            try
            {
                await _manager.Execute(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        return false;
    }


    public bool VerifyPassword(User user, string? password)
    {
        return BCrypt.Net.BCrypt.Verify(password.CalculateMD5Hash(), user.PasswordHash);
    }
}