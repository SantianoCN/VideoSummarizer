
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace VideoSummarizer;

public class AuthOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(Key??throw new InvalidOperationException("Key not defined"))
    );
}