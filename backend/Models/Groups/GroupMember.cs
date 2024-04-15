using Backend.Models.Users;

namespace Backend.Models.Groups;

public class GroupMember
{
    public required User User { get; init; }
    public required string Role { get; init; } 
}