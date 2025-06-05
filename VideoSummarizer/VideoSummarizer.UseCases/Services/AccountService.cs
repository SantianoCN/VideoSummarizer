using Microsoft.AspNetCore.Authorization;
using VideoSummarizer.Models;

namespace VideoSummarizer.UseCases.Services;

public class AccountService
{
    private JwtService _jwtService;
    public AccountService(JwtService jwtService) => (_jwtService) = (jwtService);
    public async Task RegisterUser()
    {
        
    }
    public async Task AuthorizeUser()
    {
        
    }
}