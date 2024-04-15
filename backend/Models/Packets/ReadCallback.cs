namespace Backend.Models.Packets; 

public class ReadCallback {
  public int senderID { get; set; }
  public int ChannelID { get; set; }
  public DateTimeOffset UtcTime { get; set; }
}
