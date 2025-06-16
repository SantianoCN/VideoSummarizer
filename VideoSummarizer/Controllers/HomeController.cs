using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VideoSummarizer.Models;

namespace VideoSummarizer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IHttpContextAccessor _context;

    public HomeController(IHttpContextAccessor accessor, ILogger<HomeController> logger, IHttpContextAccessor context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
