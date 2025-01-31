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
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    public FileTypeValidationMiddleware(RequestDelegate next, ILogger<FileTypeValidationMiddleware> logger, IRazorViewEngine razorViewEngine,
        ITempDataProvider tempDataProvider)
    {
        _logger = logger;
        _next = next;
        _razorViewEngine = razorViewEngine;
        _tempDataProvider = tempDataProvider;
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
                if (file.Length == 0 || file.Length > 10 * 1024 * 1024) {
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

    private async Task<string> RenderViewToStringAsync(HttpContext context, string viewName, object model)
    {
        var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());

        var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

        if (viewResult.View == null)
        {
            throw new ArgumentNullException($"{viewName} not found.");
        }

        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };

        var tempData = new TempDataDictionary(context, _tempDataProvider);

        using (var writer = new StringWriter())
        {
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                tempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }
    }
}