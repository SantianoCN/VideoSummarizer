
using Microsoft.Extensions.Options;

namespace VideoSummarizer.UseCases.Services;

public class JwtService
{
    private readonly ILogger<JwtService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IOptions<AuthOptions> _options;

    public JwtService(ILogger<JwtService> logger, IConfiguration configuration, IOptions<AuthOptions> options) =>
    (logger, configuration, options) = (_logger!, _configuration!, _options!);

    
}