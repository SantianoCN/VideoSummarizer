

namespace VideoSummarizer.Core.Entities.Database;

public class DatabaseUser
{
    public long Id { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public int Age { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    
}