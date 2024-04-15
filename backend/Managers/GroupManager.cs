using Backend.Models;
using Backend.Utils;

namespace Backend.Managers;

public class GroupManager
{
    private readonly DatabaseManager _manager;
    public GroupManager(DatabaseManager manager)
    {
        _manager = manager;
    }

    public async Task<bool> CreateGroup(string groupName, List<int> memberUserIds, int createdByUserId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@groupName", groupName },
            { "@createdBy", createdByUserId }
        };

        string groupQuery = "INSERT INTO Groups (group_name, created_by) VALUES (@groupName, @createdBy); SELECT LAST_INSERT_ID();";
        try
        {
            int groupId = await _manager.ExecuteInsert(groupQuery, parameters);
            if (groupId > 0)
            {
                foreach (int userId in memberUserIds)
                {
                    string memberQuery = "INSERT INTO GroupMembers (group_id, user_id, role) VALUES (@groupId, @userId, 'member');";
                    await _manager.Execute(memberQuery, new Dictionary<string, object>
                    {
                        { "@groupId", groupId },
                        { "@userId", userId }
                    });
                }
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteGroupById(int groupId, int userId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@groupId", groupId },
            { "@userId", userId }
        };

        string checkQuery = "SELECT 1 FROM Groups WHERE group_id = @groupId AND created_by = @userId;";
        if (await _manager.ExecuteScalar(checkQuery, parameters) != null)
        {
            string deleteQuery = "DELETE FROM Groups WHERE group_id = @groupId;";
            await _manager.Execute(deleteQuery, parameters);
            return true;
        }
        return false;
    }

    public async Task<bool> ModifyGroup(int groupId, string newName)
    {
        string query = "UPDATE Groups SET group_name = @newName WHERE group_id = @groupId;";
        var parameters = new Dictionary<string, object>
        {
            { "@groupId", groupId },
            { "@newName", newName }
        };
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
}
