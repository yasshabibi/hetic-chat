namespace Backend.Models.Packets
{
    public class Message
    {
        public string ID { get; init; }
        public string SenderID { get; init; }
        public string ReceiverID { get; init; }
        public string Content { get; init; }
        public DateTimeOffset SentUnix { get; init; }
    }
}
