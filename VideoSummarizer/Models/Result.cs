

namespace VideoSummarizer.Models;

public class Result
{
    public List<Alternative> alternatives { get; set; }
    public Usage usage { get; set; }
    public string modelVersion { get; set; } // Добавлено для modelVersion
}