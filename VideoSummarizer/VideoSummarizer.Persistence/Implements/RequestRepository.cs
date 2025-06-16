
using VideoSummarizer.Core.Contracts;
using VideoSummarizer.Core.Entities.Database;
using VideoSummarizer.Core.DTO;
using Microsoft.EntityFrameworkCore;

namespace VideoSummarizer.Persistence.Implements;

public class RequestRepository : IRepository<NewRequestDto, DatabaseSummarizeRequest>
{
    private readonly ILogger<RequestRepository> _logger;
    private readonly DataContext _context;
    public RequestRepository(ILogger<RequestRepository> logger, DataContext context) => (_logger, _context) = (logger, context);
    public async Task Create(NewRequestDto value)
    {
        var user = _context.Users.FirstOrDefault(u => u.Login == value.UserLogin);
        await _context.SummarizeRequests.AddAsync(new DatabaseSummarizeRequest
        {
            UserId = user.Id,
            Title = value.Title,
            UploadedFile = value.UploadUri,
            Transcription = value.Transcription,
            Summary = value.Summary,
            DateTime = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    public Task Delete(string uniqId)
    {
        throw new NotImplementedException();
    }

    public Task<DatabaseSummarizeRequest?> Read(string uniqId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<DatabaseSummarizeRequest>?> ReadAll(string login)
    {
        if (_context.SummarizeRequests.Count() == 0)
        {
            return null;
        }

        return await _context.SummarizeRequests.ToListAsync();
    }

    public Task Update(string uniqId, NewRequestDto value)
    {
        throw new NotImplementedException();
    }
}