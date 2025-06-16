using VideoLibrary;
using Microsoft.AspNetCore.Mvc;
using VideoSummarizer.Models;
using VideoSummarizer.UseCases.Services;
using AssemblyAI;
using Microsoft.AspNetCore.Identity;
using VideoSummarizer.Core.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace VideoSummarizer.Controllers;

[Route("VideoProcessing")]
public class VideoProcessingController : Controller
{
    private IHttpContextAccessor _accessor;
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _context;
    private readonly ITranscriptorService _transcriptor;
    private readonly ISummarizatorService _summarizator;
    private readonly RequestService _requestService;

    public VideoProcessingController(IHttpContextAccessor accessor, ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor context,
        ITranscriptorService transcriptor, ISummarizatorService summarizator, RequestService requestService)
    {
        _accessor = accessor;
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _transcriptor = transcriptor;
        _summarizator = summarizator;
        _requestService = requestService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("../Home/Index");
    }

    [HttpPost("yt")]
    public async Task<IActionResult> CreateNewSummaryFromYT([FromBody] VideoProcessingYTViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Uncurrect data");
        }

        var yt = YouTube.Default;

        var video = yt.GetVideo(model.ResourceUrl);
        string fileName = new Guid().ToString();

        await Task.Run(() =>
        {
            System.IO.File.WriteAllBytes($"~uploads/vid{fileName}.mp4", video.GetBytes());
        });

        return Ok("Ready");
    }


    [HttpPost("Upload")]
    public async Task<IActionResult> CreateNewSummaryFromFile([FromForm] IFormFile file, [FromForm] int wordsCount,
        [FromForm] bool showSourceText, [FromForm] string additionalTask){
        string text = "";
        
        var pathToUpload = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(pathToUpload, fileName);

        using (var fs = new FileStream(filePath, FileMode.Create)) {
            await file.CopyToAsync(fs);
        }
        
        text = await _transcriptor.GetTranscription(filePath);

        if (text.Split(" ").Length < 20)
        {
            return BadRequest("Текст видео слишком короткий");
        }

        if (showSourceText)
        {
            var summary = await _summarizator.GetSummary(text, wordsCount, additionalTask);

            if (string.IsNullOrEmpty(summary)) return BadRequest();

            await _requestService.SaveRequest(new Core.DTO.NewRequestDto
            {
                Title = summary.Split(' ')[0],
                UserLogin = _accessor.HttpContext.User.Identity.Name,
                UploadUri = filePath,
                Summary = text,
                Transcription = text
            });

            

            return Json(new
            {
                summary = summary,
                sourceText = text
            });
        }
        else
        {
            var summary = await _summarizator.GetSummary(text, wordsCount, additionalTask);

            if (string.IsNullOrEmpty(summary)) return BadRequest();

            await _requestService.SaveRequest(new Core.DTO.NewRequestDto
            {
                Title = "",
                UserLogin = User.Identity?.Name,
                UploadUri = filePath,
                Summary = text,
                Transcription = ""
            });

            

            return Json(new
            {
                summary = summary,
                sourceText = ""
            });
        }
    }
}