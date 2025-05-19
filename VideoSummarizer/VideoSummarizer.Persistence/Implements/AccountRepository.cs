
using VideoSummarizer.Core.Contracts;
using VideoSummarizer.Core.Entities.Database;
using VideoSummarizer.Persistence.DTO;

namespace VideoSummarizer.Persistence.Implements;

public class AccountRepository : IRepository<AccountUserDTO>
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

    public Task Delete(string uniqId)
    {
        throw new NotImplementedException();
    }

    public Task<AccountUserDTO> Read()
    {
        throw new NotImplementedException();
    }

    public Task Update<UpdateDTO>(string uniqId, UpdateDTO value)
    {
        throw new NotImplementedException();
    }
}