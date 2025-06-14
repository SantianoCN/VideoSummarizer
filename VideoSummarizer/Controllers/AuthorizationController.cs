using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using VideoSummarizer.Core.DTO;
using VideoSummarizer.Models;
using VideoSummarizer.UseCases.Services;

namespace VideoSummarizer.Controllers;

[Route("[controller]")]
public class AuthorizationController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IHttpContextAccessor _context;
    private AccountService _accountService;
    private HashService _hashService;

    public AuthorizationController(
        ILogger<HomeController> logger, 
        IHttpContextAccessor context, 
        AccountService accountService,
        HashService hashService) =>
        (_logger, _context, _accountService, _hashService) = 
        (logger, context, accountService, hashService);

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromForm] string login, [FromForm] string password)
    {
        Stack<string> errors = new Stack<string>();
        string token = "";
        
        _accountService.AuthorizeUser(new UserAuthDTO
        {
            Login = login,
            Password = password
        }, ref token, ref errors);

        if (string.IsNullOrEmpty(token)) 
        {
            _context.HttpContext.Response.StatusCode = 403;
            return BadRequest(errors.Count > 0 ? errors.Pop() : "Invalid login or password");
        }

        _context.HttpContext.Response.Headers.Add("Authorization", $"Bearer: {token}");
        return Ok();
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] UserRegistrationModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data provided");
        }

        if (model.Password != model.ConfirmPassword)
        {
            return BadRequest("Passwords do not match");
        }

        // Check if user already exists
        if (await _accountService.IsUserExists(model.Login))
        {
            return BadRequest("User with this login already exists");
        }

        // Hash the password
        var (hash, salt) = await _hashService.HashPassword(model.Password);

        // Create user in database (implementation depends on your repository)
        var result = await _accountService.RegisterUser(new UserRegisterDto
        {
            Username = model.Username,
            Login = model.Login,
            Password = hash,
            Salt = salt
        });

        if (!result)
        {
            return StatusCode(500, "Failed to create user");
        }

        return Ok();
    }
}