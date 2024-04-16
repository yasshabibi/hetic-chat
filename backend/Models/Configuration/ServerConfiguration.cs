namespace Backend.Models.Configuration;

public class ServerConfiguration
{
  public string Server { get; set; } = "127.0.0.1";
  public string Database { get; set; } = "sys";
  public string Username { get; set; } = "root";
  public string Password { get; set; } = "root";
}
