

namespace VideoSummarizer.Core.Contracts;


public interface IRepository<DTO>
{
    Task Create(DTO value);
    Task<DTO> Read();
    Task Update<UpdateDTO>(string uniqId, UpdateDTO value);
    Task Delete(string uniqId);
}