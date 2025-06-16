using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using VideoSummarizer.Core.DTO;
using VideoSummarizer.Persistence.Implements;

namespace VideoSummarizer.UseCases.Services;

public class AccountService
{
    private readonly AccountRepository _repository;
    private readonly JwtService _jwtService;
    private readonly HashService _hashService;
    
    public AccountService(
        AccountRepository repository, 
        JwtService jwtService, 
        HashService hashService) => 
        (_repository, _jwtService, _hashService) = (repository, jwtService, hashService);
    
    public async Task<bool> IsUserExists(string login)
    {
        return await _repository.IsExists(login);
    }

    public async Task<bool> RegisterUser(UserRegisterDto model)
    {
        try
        {
            await _repository.Create(model);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
    public void AuthorizeUser(UserAuthDTO model, ref string token, ref Stack<string> errors)
    {
        if (! _repository.IsExists(model.Login).Result)
        {
            errors.Push("User not found");
            return;
        }

        var user = _repository.Read(model.Login).Result;

        if (_hashService.VerifyPassword(user.Password, model.Password, user.Salt).Result)
        {

            token = _jwtService.GenerateSecurityToken(model, user.UserId).Result;
        }
        else
        {
            errors.Push("Invalid login or password");
        }
    }
}