
using VideoLibrary;
using Microsoft.AspNetCore.Mvc;
using VideoSummarizer.Models;
using VideoSummarizer.Services;
using AssemblyAI;
using Microsoft.AspNetCore.Identity;

namespace VideoSummarizer.Controllers;

[Route("VideoProcessing")]
public class VideoProcessingController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _context;
    private readonly ITranscriptorService _transcriptor;
    private readonly ISummarizatorService _summarizator;
    
    public VideoProcessingController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor context, ITranscriptorService transcriptor, ISummarizatorService summarizator) {
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _transcriptor = transcriptor;
        _summarizator = summarizator;
    }
    [HttpGet]
    public IActionResult Index(){
        return View("../Home/Index");
    }

    [HttpPost("yt")]
    public async Task<IActionResult> CreateNewSummaryFromYT([FromBody] VideoProcessingYTViewModel model) 
    {
        if (!ModelState.IsValid){
            return BadRequest("Uncurrect data");
        }

        var yt = YouTube.Default;
        
        var video = yt.GetVideo(model.ResourceUrl);
        string fileName = new Guid().ToString();
        
        await Task.Run(() => {
            System.IO.File.WriteAllBytes($"~uploads/vid{fileName}.mp4", video.GetBytes());
        });
        
        return Ok("Ready");
    }


    [HttpPost("upload")]
    public async Task<IActionResult> CreateNewSummaryFromFile(IFormFile file){
        string text = "";

        // var modelTest = new TranscriptionViewModel {
        //     Text = "This is some long long text, for testing, how it work."
        // };
        // Thread.Sleep(5000);

        // return Json(modelTest);

        using (var fileStream = new FileStream("../uploads", FileMode.Create, FileAccess.Write)) {
            await file.CopyToAsync(fileStream);
        }
        using (var fileStream = new FileStream("../uploads", FileMode.Open, FileAccess.Read)) {
            var task = Task.Run(async () => {
                return await _transcriptor.GetTranscription(fileStream);
            });

            await task.ContinueWith((task) => {
                fileStream.Close();
            });

            text = task.Result;
        }
        
        if (text.Split(" ").Length < 20) {
            return BadRequest("Текст видео слишком короткий");
        }

        return Json(new {
            text = await _summarizator.GetSummary(text, 0, "")
        });
    }
}