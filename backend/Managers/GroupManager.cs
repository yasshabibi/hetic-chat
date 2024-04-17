using Backend.Models;
using Backend.Models.Groups;
using Backend.Models.Users;
using Backend.Utils;
using MySqlConnector;

namespace Backend.Managers;

public class GroupManager
{
    private readonly DatabaseManager _manager;
    private readonly UserManager _userManager;
    public GroupManager(DatabaseManager manager, UserManager userManager)
    {
        _manager = manager;
        _userManager = userManager;
    }

    public async Task<List<Group>> GetGroupByUserId(int userId)
    {
        var query = @"
        SELECT g.group_id, g.group_name, g.created_by, gm.user_id, u.username, gm.role
        FROM Groups g
        JOIN GroupMembers gm ON g.group_id = gm.group_id
        JOIN Users u ON gm.user_id = u.user_id
        WHERE gm.user_id = @userId;";

        var parameters = new Dictionary<string, object> { { "@userId", userId } };
        var fetch = await _manager.FetchAll<object>(query, parameters);

        var groups = new List<Group>();
        var groupDictionary = new Dictionary<int, Group>();

        foreach (var row in fetch)
        {
            int groupId = Convert.ToInt32(row["group_id"]);
            if (!groupDictionary.ContainsKey(groupId))
            {
                groupDictionary[groupId] = new Group
                {
                    GroupId = groupId,
                    GroupName = row["group_name"] as string ?? "",
                    CreatedBy = Convert.ToInt32(row["created_by"]),
                    Members = new List<GroupMember>()
                };
            }

            User u = (await _userManager.GetUserById(Convert.ToInt32(row["user_id"])))!;
            
            groupDictionary[groupId].Members.Add(new GroupMember
            {
                User = u,
                Role = row["role"] as string ?? "member"  // Assuming default role is 'member'
            });
        }

        return groupDictionary.Values.ToList();
    }


    public async Task<bool> CreateGroup(string groupName, List<int> memberUserIds, int createdByUserId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@groupName", groupName },
            { "@createdBy", createdByUserId }
        };

        string groupQuery = "INSERT INTO `Groups` (group_name, created_by) VALUES (@groupName, @createdBy); SELECT LAST_INSERT_ID();";
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

        string checkQuery = "SELECT 1 FROM `Groups` WHERE group_id = @groupId AND created_by = @userId;";
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
        string query = "UPDATE `Groups` SET group_name = @newName WHERE group_id = @groupId;";
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
