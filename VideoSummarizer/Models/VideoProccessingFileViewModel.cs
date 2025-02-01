using System.Collections.ObjectModel;

namespace VideoSummarizer.Models;

public class VideoProcessingFileViewModel {
    public IFormFile Files { get; set; }
    public int WordsCount { get; set; }
    public bool ShowSourceText { get; set; }
    public string Options { get; set; }
}