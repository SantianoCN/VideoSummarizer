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
    public async Task<string> GetTranscription(string filePath)
    {
        if (!File.Exists(filePath)) {
            throw new Exception("File is not exist");
        }

        var uploadedFile = await _client.Files.UploadAsync(new FileInfo(filePath));
        var fileUrl = uploadedFile.UploadUrl;

        try {

            var transcriptionOptions = new TranscriptParams {
                AudioUrl = fileUrl,
                LanguageCode = TranscriptLanguageCode.Ru
            };
            
            var transcripts = await _client.Transcripts.TranscribeAsync(transcriptionOptions);

            transcripts.EnsureStatusCompleted();
            
            return transcripts.Text;
        } catch (Exception ex) {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}