
using System.ComponentModel.DataAnnotations;

namespace VideoSummarizer.Models;

public class VideoProcessingYTViewModel {
    [Required(ErrorMessage = "Поле ссылка является обязательным")]
    public string? ResourceUrl { get; set; }
    public int WordsCount { get; set; }
    public bool GetFullText { get; set; }
}