
namespace VideoSummarizer.Models;

public class RequestRoot
{
    public string modelUri { get; set; }
    public RequestCompletionOptions completionOptions { get; set; }
    public List<Message> messages { get; set; }
}