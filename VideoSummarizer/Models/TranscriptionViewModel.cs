using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoSummarizer.Models;

public class TranscriptionViewModel : PageModel
{
    public string? Text { get; set; }
}