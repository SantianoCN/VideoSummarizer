
namespace VideoSummarizer.Services;

public interface ITranscriptorService {
    Task<string> GetTranscription(FileStream fileStream);
}