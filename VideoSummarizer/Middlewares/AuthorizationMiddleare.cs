using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace VideoSummarizer.Middlewares;

class AuthorizationMiddleware {
    private readonly string _token;
    private readonly RequestDelegate _next;
    public AuthorizationMiddleware(RequestDelegate next, string token){
        _next = next;
        _token = token;
    }
    
    public async Task Invoke(HttpContext context){
        if (context.Request.Path.Value.Contains("Authorization")) {
            await _next.Invoke(context);
            return;
        }
        if (context.Request.Cookies["token"] == _token){
            await _next.Invoke(context);
        } else {
            await context.Response.WriteAsync("403 Access denied");
        }
    }
}