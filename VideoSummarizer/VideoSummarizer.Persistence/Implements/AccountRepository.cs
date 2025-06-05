
using VideoSummarizer.Core.Contracts;
using VideoSummarizer.Core.Entities.Database;
using VideoSummarizer.Persistence.DTO;

namespace VideoSummarizer.Persistence.Implements;

public class AccountRepository : IRepository<AccountUserDTO, AccountUserReadDTO, AccountUserUpdateDTO>
{
    private readonly ILogger<AccountRepository> _logger;
    private readonly DataContext _context;
    public AccountRepository(ILogger<AccountRepository> logger, DataContext context) => (logger, context) = (_logger, _context);
    public async Task Create(AccountUserDTO value)
    {
        _context.Users.Add(new DatabaseUser
        {
            UserId = Guid.NewGuid().ToString(),
            Username = value.Username,
            Login = value.Login,

        });
    }

    public async Task Delete(string uniqId)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == uniqId);
        if (user != null) {
            _context.Users.Remove(user);
        }
    }

    public async Task<AccountUserReadDTO?> Read(string uniqId)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == uniqId);
        if (user != null)
        {
            return new AccountUserReadDTO
            {
                Username = user.Username,
                Age = user.Age
            };
        }
        return null;
    }

    public async Task Update(string uniqId, AccountUserUpdateDTO value)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == uniqId);
        if (user != null)
        {
            user.GetType().GetProperty(value.AttributeName).SetValue(user, value);
        }
    }
}