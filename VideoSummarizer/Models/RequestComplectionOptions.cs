
namespace VideoSummarizer.Models;

public class RequestCompletionOptions
{
    public bool stream { get; set; }
    public double temperature { get; set; }
    public string maxTokens { get; set; }
}