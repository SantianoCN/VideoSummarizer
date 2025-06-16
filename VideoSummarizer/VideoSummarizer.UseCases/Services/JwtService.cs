
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VideoSummarizer.Core.DTO;

namespace VideoSummarizer.UseCases.Services;

public class JwtService
{
    private readonly ILogger<JwtService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IOptions<AuthOptions> _options;
    private readonly TokenValidationParameters _tokenParameters;

    public JwtService(ILogger<JwtService> logger, IOptions<AuthOptions> options, TokenValidationParameters tokenParameters) =>
        (_logger, _options, _tokenParameters) = (logger!, options!, tokenParameters!);

    public async Task<string> GenerateSecurityToken(UserAuthDTO user, string userId)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = new JwtSecurityToken(
            issuer: _options.Value.Issuer,
            audience: _options.Value.Audience,
            claims: new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer)
            },
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: new SigningCredentials(_options.Value.GetSymmetricSecurityKey(), algorithm: SecurityAlgorithms.HmacSha256)
        );


        return handler.WriteToken(token);
    }

    public bool ValidateToken(string token, [Optional] out ClaimsPrincipal? principal)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            principal = tokenHandler.ValidateToken(token, _tokenParameters, out _);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            principal = null;
            return false;
        }
    }
}