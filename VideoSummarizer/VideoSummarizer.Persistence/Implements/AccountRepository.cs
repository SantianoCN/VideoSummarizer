
using VideoSummarizer.Core.Contracts;
using VideoSummarizer.Core.DTO;
using VideoSummarizer.Core.Entities.Database;

namespace VideoSummarizer.Persistence.Implements;

public class AccountRepository : IRepository<UserRegisterDto, DatabaseUser>
{
    private readonly ILogger<AccountRepository> _logger;
    private readonly DataContext _context;
    public AccountRepository(ILogger<AccountRepository> logger, DataContext context) => (_logger, _context) = (logger, context);
    public async Task Create(UserRegisterDto value)
    {
        await _context.Users.AddAsync(new DatabaseUser
        {
            UserId = Guid.NewGuid().ToString(),
            Username = value.Username,
            Login = value.Login,
            Password = value.Password,
            Salt = value.Salt
        });

        await _context.SaveChangesAsync();
    }

    public async Task Delete(string uniqId)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == uniqId);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }

    public async Task<DatabaseUser?> Read(string login)
    {
        var user = _context.Users.FirstOrDefault(u => u.Login == login);
        if (user != null)
        {
            return user;
        }
        return null;
    }

    public async Task Update(string uniqId, UserRegisterDto value)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == uniqId);
        if (user != null)
        {
        }
    }

    public async Task<bool> IsExists(string login)
    {
        if (_context.Users.Any(u => u.Login == login))
        {
            return true;
        }
        return false;
    }
}