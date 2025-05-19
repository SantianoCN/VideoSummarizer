using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VideoSummarizer;


public static class AuthServiceExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,

                ValidIssuer = "copyright(c)VideoSummarizer2025",
                ValidAudience = "ApplicationUser",

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthorizationSettings:Key"]!))
            };
        });

        return collection;
    }
}