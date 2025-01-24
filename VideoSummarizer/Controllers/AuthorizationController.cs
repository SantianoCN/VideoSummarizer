using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace VideoSummarizer.Controllers;

public class AuthorizationController : Controller {
    private readonly ILogger<HomeController> _logger;
    private IHttpContextAccessor _context;

    public AuthorizationController(ILogger<HomeController> logger, IHttpContextAccessor context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index(){
        return View();
    }

    [HttpPost]
    public IActionResult SignIn(){
        var login = _context.HttpContext.Request.Form["login"];
        var pass = _context.HttpContext.Request.Form["pass"];

        if (login == "admin" && pass == "123") {
            _context.HttpContext.Response.Cookies.Append("token", "123");
            return Redirect("/Home/Index");
        } else {
            return Ok();
        }
    }
}