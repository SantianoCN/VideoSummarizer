

using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using VideoSummarizer.UseCases.Services;

namespace VideoSummarizer.Middlewares;

public class AuthenticationMiddleware
{
    private readonly ILogger<AuthenticationMiddleware> _logger;
    private readonly RequestDelegate _next;
    public AuthenticationMiddleware(ILogger<AuthenticationMiddleware> logger) => (_logger) = (logger);
    public async Task InvokeAsync(HttpContext context, JwtService jwtService)
    {
        StringValues token;
        if (context.Request.Headers.TryGetValue("Authorization", out token))
        {
            if (!string.IsNullOrEmpty(token.ToString()) && !string.IsNullOrEmpty(token.ToString().Replace("Bearer: ", "")))
            {
                ClaimsPrincipal? principal;
                if (jwtService.ValidateToken(token, out principal) && principal != null)
                {
                    context.User = principal;
                    await _next(context);
                    return;
                }
            }
        }
        else if (context.Request.Cookies.TryGetValue("jwtToken", out var tokenCookie))
        {
            ClaimsPrincipal principal;
            if (jwtService.ValidateToken(tokenCookie, out principal) && principal != null)
            {
                context.User = principal;
                await _next(context);
                return;
            }
        }
        
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
    }
}