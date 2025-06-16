

namespace VideoSummarizer.Core.DTO;

public class NewRequestDto
{
    public string? Title { get; set; }
    public string UserLogin { get; set; }
    public string UploadUri { get; set; }
    public string Summary { get; set; }
    public string Transcription { get; set; }
}