
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VideoSummarizer.UseCases.Services;

namespace VideoSummarizer.Controllers;

[Route("[controller]")]
public class HistoryController : Controller
{
    private readonly IHttpContextAccessor _accessor;
    private readonly ILogger<HistoryController> _logger;
    private IHttpContextAccessor _context;
    private AccountService _accountService;
    private RequestService _requestService;

    public HistoryController(
        IHttpContextAccessor accessor,
        ILogger<HistoryController> logger,
        IHttpContextAccessor context,
        AccountService accountService,
        RequestService requestService) =>
        (_accessor, _logger, _context, _accountService, _requestService) =
        (accessor, logger, context, accountService, requestService);

    [HttpGet]
    public async Task<IActionResult> GetUserRequests()
    {
        try
        {
            var userLogin = _accessor.HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(userLogin))
            {
                return Unauthorized(new { Message = "User not authenticated" });
            }

            var requests = await _requestService.GetAllRequests(userLogin);

            return new JsonResult(new
            {
                Success = true,
                Data = requests
            })
            {
                ContentType = "application/json"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user requests");
            return StatusCode(500, new
            {
                Success = false,
                Message = "Internal server error"
            });
        }
    }       
}