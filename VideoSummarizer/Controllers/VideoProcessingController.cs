
using VideoLibrary;
using Microsoft.AspNetCore.Mvc;
using VideoSummarizer.Models;
using VideoSummarizer.Services;
using AssemblyAI;
using Microsoft.AspNetCore.Identity;

namespace VideoSummarizer.Controllers;

[Route("VideoProcessing")]
public class VideoProcessingController : Controller
{
    private IFormFile file;
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _context;
    private readonly ITranscriptorService _transcriptor;
    private readonly ISummarizatorService _summarizator;

    public VideoProcessingController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor context, ITranscriptorService transcriptor, ISummarizatorService summarizator)
    {
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _transcriptor = transcriptor;
        _summarizator = summarizator;
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


    [HttpPost("upload")]
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
            return Json(new
            {
                summary = await _summarizator.GetSummary(text, wordsCount, additionalTask),
                sourceText = text
            });
        }
        else
        {
            return Json(new
            {
                summary = await _summarizator.GetSummary(text, wordsCount, additionalTask),
                sourceText = ""
            });
        }
    }
}