

using VideoSummarizer.Persistence.DTO;

namespace VideoSummarizer.Core.Contracts;


public interface IRepository<CrDTO, RdDTO, UpdDTO>
{
    Task Create(CrDTO value);
    Task<RdDTO?> Read(string uniqId);
    Task Update(string uniqId, UpdDTO value);
    Task Delete(string uniqId);
}