

namespace VideoSummarizer.Core.Contracts;


public interface IRepository<CrDTO, DatabaseEntity>
{
    Task Create(CrDTO value);
    Task<DatabaseEntity?> Read(string uniqId);
    Task Update(string uniqId, CrDTO value);
    Task Delete(string uniqId);
}