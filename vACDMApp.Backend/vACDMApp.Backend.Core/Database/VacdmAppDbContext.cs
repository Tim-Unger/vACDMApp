using DotNetEnv;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace vACDMApp.Backend.Core.Database;

public class VacdmAppDbContext : DbContext
{
    public VacdmAppDbContext() { }

    public VacdmAppDbContext(DbContextOptions<VacdmAppDbContext> options) : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Env.Load();

        var postgresDatabase = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var postgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");

        var includeErrors = "";

#if DEBUG
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
        includeErrors = "Include Error Detail=True";
        optionsBuilder.ConfigureWarnings(w => w.Log(RelationalEventId.MultipleCollectionIncludeWarning));
#endif

        //We need the preprocessor directives since ef core can't read the env variables when running from the cli
        if (postgresDatabase is null)
        {
#if DEBUG
            postgresDatabase = "rapidhotel";
#else
                throw new InvalidOperationException("POSTGRES_DB is not set");
#endif
        }

        if (postgresUser is null)
        {
#if DEBUG
            postgresUser = "rapidhotel";
#else
                throw new InvalidOperationException("POSTGRES_USER is not set");
#endif
        }

        if (postgresPassword is null)
        {
#if DEBUG
            postgresPassword = "rapidhotel";
#else
                throw new InvalidOperationException("POSTGRES_PASSWORD is not set");
#endif
        }

        if (postgresHost is null)
        {
            Log.Warning("No POSTGRES_HOST configured, defaulting to postgres");
            //Assume runnning in docker
            postgresHost = "postgres";
        }

        if (postgresPort is null)
        {
            Log.Warning("No POSTGRES_POST configured, defaulting to 5432");
            postgresPort = "5432";
        }

#if RELEASE
            // if(postgresPassword.Length < 12)
            // {
            //     throw new InvalidOperationException("POSTGRES_PASSWORD is too short");
            // }
#endif
        var connectionString =
            $"Host={postgresHost};Port={postgresPort};Database={postgresDatabase};User ID={postgresUser};Password={postgresPassword};Maximum Pool Size=200;Minimum Pool Size=10;{includeErrors}";

        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        
        optionsBuilder.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}