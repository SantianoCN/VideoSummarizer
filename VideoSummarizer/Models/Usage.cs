
namespace VideoSummarizer.Models;

public class Usage
{
    public string inputTextTokens { get; set; }
    public string completionTokens { get; set; }
    public string totalTokens { get; set; }
    public object completionTokensDetails { get; set; } // object, так как значение null
}