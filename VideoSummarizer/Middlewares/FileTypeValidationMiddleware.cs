using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System.Security.Principal;
using System.Text.RegularExpressions;
using VideoSummarizer.Models;

namespace VideoSummarizer.Middlewares;


public class FileTypeValidationMiddleware
{
    private readonly ILogger<FileTypeValidationMiddleware> _logger;
    private readonly RequestDelegate _next;
    public FileTypeValidationMiddleware(RequestDelegate next, ILogger<FileTypeValidationMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/VideoProcessing/upload")
        {
            IFormFileCollection files = context.Request.Form.Files;

            foreach (var file in files)
            {
                var ext = Regex.Match(file.FileName, @"\.[^.]*$").Value.ToLower();
                if (ext != ".mp3" && ext != ".wav" && ext != ".aiff" && ext != ".ogg" && ext != ".mp4" && ext != ".avi" && ext != ".mpeg-4" && ext != "mov")
                {
                    var message = "File type not supported";
                    _logger.LogError(message);

                    var model = new
                    {
                        Message = message
                    };

                    context.Response.StatusCode = 415;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(model);
                    return;
                }
                if (file.Length == 0 || file.Length > 100 * 1024 * 1024) {
                     var message = "File size must be not over 100mb";
                    _logger.LogError(message);

                    var model = new
                    {
                        Message = message
                    };

                    context.Response.StatusCode = 413;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(model);
                    return;
                }
            }  await _next.Invoke(context);
        }
        else await _next.Invoke(context);
    }
}