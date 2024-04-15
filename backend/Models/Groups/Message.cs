using Backend.Models.Users;

namespace Backend.Models.Groups
{
    public class Message
    {
        public required int ID { get; init; }
        public required int GroupID { get; init; }
        public required int UserID { get; init; }
        public required string Content { get; init; }
        public required DateTime Timestamp { get; init; }
    }
}
