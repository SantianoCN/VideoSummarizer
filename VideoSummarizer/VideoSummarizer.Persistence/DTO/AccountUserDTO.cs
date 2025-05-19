
using Microsoft.AspNetCore.Identity.Data;

namespace VideoSummarizer.Persistence.DTO;

public class AccountUserDTO
{
    public string Username { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
}