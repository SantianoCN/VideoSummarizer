using System.Threading.Tasks;
using AssemblyAI;
using AssemblyAI.Transcripts;
using VideoLibrary.Exceptions;

namespace VideoSummarizer.Services;

public class TranscriptorService : ITranscriptorService
{
    private readonly ILogger<TranscriptorService> _logger;
    private readonly AssemblyAIClient _client;
    public TranscriptorService(ILogger<TranscriptorService> logger, AssemblyAIClient client) {
        _logger = logger;
        _client = client;   
    }
    public async Task<string> GetTranscription(FileStream fileStream)
    {
        if (!fileStream.CanRead) {
            throw new Exception("File stream must be readable");
        }

        try {
            var transcripts = await _client.Transcripts.TranscribeAsync(fileStream);

            transcripts.EnsureStatusCompleted();
            
            return transcripts.Text;
        } catch (Exception ex) {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}