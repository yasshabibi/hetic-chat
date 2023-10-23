using Backend.Enums;
using Backend.Models.Users;
using Backend.Utils;

namespace Backend.Managers;

public class UserManager
{

    private DatabaseManager _dbManager;

    public UserManager(DatabaseManager dbManager)
    {
        _dbManager = dbManager;
    }

    /// <summary>
    /// Get all users matching the display name
    /// </summary>
    /// <param name="displayName">display name to search in</param>
    /// <returns>list of users matching GetAllUsers</returns>
    public async Task<IEnumerable<User>> GetAllUsersMatching(string displayName)
    {
        var users = await _dbManager.FetchAll("SELECT * FROM users WHERE display_name LIKE @login",
            new Dictionary<string, object>() { ["login"] = $"%{displayName}%" });

        return users.Select(s => FromSQLDict(s));
    }

    /// <summary>
    /// Get All users
    /// </summary>
    /// <returns>All users</returns>
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var users = await _dbManager.FetchAll("SELECT * FROM users");

        return users.Select(s => FromSQLDict(s));
    }

    

    /// <summary>
    /// Get a user by it's login
    /// </summary>
    /// <param name="login">login name of the user</param>
    /// <returns>the specific user</returns>
    public async Task<User?> GetUserByLogin(string login, bool showPassword = false)
    {
        var user = await _dbManager.FetchOne("SELECT * FROM users WHERE login_name = @login", 
            new Dictionary<string, object>() { ["login"] = login });

        if (user == null)
            return null;

        return FromSQLDict(user);
    }

    /// <summary>
    /// Get a user by it's ID
    /// </summary>
    /// <param name="id">id of the user</param>
    /// <returns>the user</returns>
    public async Task<User?> GetUserById(int id, bool showPassword = false)
    {
        var user = await _dbManager.FetchOne("SELECT * FROM users WHERE id = @user",
            new Dictionary<string, object> { ["user"] = id });
        if (user == null)
            return null;

        return FromSQLDict(user, showPassword);

    }


    private User FromSQLDict(Dictionary<string, object> user, bool showPassword = false)
    {
        return new User()
        {
            ID = (int)user["id"],
            LoginName = (string)user["login_name"],
            DisplayName = (string)user["display_name"],
            PasswordHash = showPassword ? (string)user["pw_hash"] : "secret potato",
            Rank = (UserRank)user["rank"]
        };
    }


}