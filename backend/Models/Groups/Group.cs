namespace Backend.Models.Groups;

public class Group
{
    public required int GroupId { get; init; }
    public required string GroupName { get; init; }
    public required int CreatedBy { get; init; }
    public required List<GroupMember> Members { get; init; } = new();
}