
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using VideoSummarizer.Core.Entities.Database;

namespace VideoSummarizer.Persistence;

public class DataContext : DbContext
{
    private readonly ILogger<DataContext> _logger;
    private readonly IConfiguration _configuration;
    public DataContext(ILogger<DataContext> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        Database.EnsureCreated();
    }
    public DbSet<DatabaseUser> Users { get; set; }
    public DbSet<DatabaseSummarizeRequest> SummarizeRequests { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration["ConnectionStrings:Default"]);
    }
}