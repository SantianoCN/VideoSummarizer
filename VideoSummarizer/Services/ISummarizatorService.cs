
namespace VideoSummarizer.Services;

public interface ISummarizatorService {
    Task<string> GetSummary(string sourceText, int wordsCount, string options);
}