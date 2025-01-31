

using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using VideoSummarizer.Models;

namespace VideoSummarizer.Services;

public class SummarizatorService : ISummarizatorService
{
    private readonly ILogger<SummarizatorService> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _client;
    public SummarizatorService(ILogger<SummarizatorService> logger, IConfiguration configuration, HttpClient client)
    {
        _logger = logger;
        _configuration = configuration;
        _client = client;
    }

    public async Task<string> GetSummary(string sourceText, int wordsCount, string options)
    {
        if (sourceText is null || sourceText == "")
        {
            throw new NullReferenceException("Source text is null");
        }

        var url = _configuration["ApiUrls:Summarization"];
        var requestUrl = _configuration["ApiUrls:SummarizationRequestUrl"];

        var task = "Суммаризируй приведенный текст.";

        if (wordsCount > 0)
        {
            task += $"Максимальное количество слов в получившемся тексте не более: {wordsCount} слов.";
        }
        if (options != "")
        {
            task += $"Так же нужно выполнить следующие операции с текстом: {options}";
        }

        // var prompt = new
        // {
        //     modelUri = requestUrl,
        //     completionOptions = new
        //     {
        //         stream = false,
        //         temperature = 0.15,
        //         maxTokens = "2000"
        //     },
        //     messages = new[] {
        //         new {
        //             role = "system",
        //             text = task.ToString()
        //         },
        //         new {
        //             role = "user",
        //             text = sourceText
        //         }
        //     }
        // };
        var textTask = task.ToString();
        var prompt = new RequestRoot
        {
            modelUri = requestUrl,
            completionOptions = new RequestCompletionOptions
            {
                stream = false,
                temperature = 0.15,
                maxTokens = "2000"
            },
            messages = new List<Message> {
                new Message {
                    role = "system",
                    text = task
                },
                new Message {
                    role = "user",
                    text = sourceText
                }
            }
        };

        var jsonOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var result = await _client.PostAsJsonAsync(url, prompt, jsonOptions);

        if (result.IsSuccessStatusCode)
        {
            var fromRequestJson = await result.Content.ReadAsStringAsync();
            Root model = JsonSerializer.Deserialize<Root>(fromRequestJson)!;

            return model.result.alternatives[0].message.text;

        }
        else
        {
            var error = await result.Content.ReadAsStringAsync();
            return "";
        }
    }
}