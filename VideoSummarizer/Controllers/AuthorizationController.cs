using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using VideoSummarizer.Models;

using VideoSummarizer.UseCases.Services;

namespace VideoSummarizer.Controllers;

public class AuthorizationController : Controller {
    private readonly ILogger<HomeController> _logger;
    private IHttpContextAccessor _context;
    private AccountService _accountService;

    public AuthorizationController(ILogger<HomeController> logger, IHttpContextAccessor context, AccountService accountService) =>
        (_logger, _context, _accountService) = (logger, context, accountService);

    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    public IActionResult SignIn(UserAuthorizationModel model)
    {
        return View();
    }
}