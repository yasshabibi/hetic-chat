using Backend.Models;
using Backend.Models.Groups;
using Backend.Utils;

namespace Backend.Managers;

public class MessageManager
{
    private readonly DatabaseManager _manager;
    public MessageManager(DatabaseManager manager)
    {
        _manager = manager;
    }

    public async Task<bool> AddMessage(int groupId, int userId, string content)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@groupId", groupId },
            { "@userId", userId },
            { "@content", content }
        };

        string query = "INSERT INTO Messages (group_id, user_id, content) VALUES (@groupId, @userId, @content);";
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

    public async Task<List<Message>> GetMessages(int groupId)
    {
        string query = "SELECT message_id, user_id, content, timestamp FROM Messages WHERE group_id = @groupId;";
        var parameters = new Dictionary<string, object> { { "@groupId", groupId } };

        var fetch = await _manager.FetchAll<object>(query, parameters);
        var messages = new List<Message>();
        foreach (var row in fetch)
        {
            messages.Add(new Message
            {
                ID = Convert.ToInt32(row["message_id"]),
                GroupID = groupId,
                UserID = Convert.ToInt32(row["user_id"]),
                Content = row["content"].ToString() ?? string.Empty,
                Timestamp = Convert.ToDateTime(row["timestamp"])
            });
        }
        return messages;
    }

    public async Task<bool> DeleteMessage(int messageId, int userId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@messageId", messageId },
            { "@userId", userId }
        };

        string checkQuery = "SELECT 1 FROM Messages WHERE message_id = @messageId AND (user_id = @userId OR user_id IN (SELECT created_by FROM Groups JOIN Messages ON Groups.group_id = Messages.group_id WHERE message_id = @messageId));";
        if (await _manager.ExecuteScalar(checkQuery, parameters) != null)
        {
            string deleteQuery = "DELETE FROM Messages WHERE message_id = @messageId;";
            await _manager.Execute(deleteQuery, parameters);
            return true;
        }
        return false;
    }
}
