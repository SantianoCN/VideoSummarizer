
using VideoSummarizer.Core.DTO;
using VideoSummarizer.Core.Entities.Database;
using VideoSummarizer.Persistence;
using VideoSummarizer.Persistence.Implements;

namespace VideoSummarizer.UseCases.Services;

public class RequestService
{
    public ILogger<RequestService> _logger;
    public IConfiguration _configuration;
    public AccountRepository _accountRepository;
    public RequestRepository _requestRepository;
    public RequestService(ILogger<RequestService> logger, IConfiguration configuration, AccountRepository repository, RequestRepository requestRepository) =>
        (_logger, _configuration, _accountRepository, _requestRepository) = (logger, configuration, repository, requestRepository);

    public async Task SaveRequest(NewRequestDto newRequest)
    {
        if (!File.Exists(newRequest.UploadUri))
        {
            return;
        }

        if (!await _accountRepository.IsExists(newRequest.UserLogin))
        {
            return;
        }

        try
        {
            await _requestRepository.Create(newRequest);
        }
        catch
        {
            return;
        }
    }

    public async Task<IEnumerable<DatabaseSummarizeRequest>> GetAllRequests(string userLogin)
{
    try
    {
        if (!await _accountRepository.IsExists(userLogin))
        {
            _logger.LogWarning($"User {userLogin} not found");
            return Enumerable.Empty<DatabaseSummarizeRequest>();
        }

        return await _requestRepository.ReadAll(userLogin);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Error getting requests for user {userLogin}");
        return Enumerable.Empty<DatabaseSummarizeRequest>();
    }
}
}