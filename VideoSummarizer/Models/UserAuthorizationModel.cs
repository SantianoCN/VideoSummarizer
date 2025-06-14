
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VideoSummarizer.Models;

public class UserAuthorizationModel : Model
{
    public string Login { get; set; }
    public string Password { get; set; }
}