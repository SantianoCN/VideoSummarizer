
namespace VideoSummarizer.Core.Entities.Database;

public class DatabaseSummarizeRequest
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public DatabaseUser User { get; set; }
    public string Title { get; set; }
    public string UploadedFile { get; set; }
    public string Transcription { get; set; }
    public string Summary { get; set; }
    public DateTime DateTime { get; set; }
}