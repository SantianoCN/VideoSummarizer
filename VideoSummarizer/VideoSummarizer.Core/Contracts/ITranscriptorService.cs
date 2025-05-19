
namespace VideoSummarizer.Core.Contracts;

public interface ITranscriptorService {
    Task<string> GetTranscription(string filePath);
}