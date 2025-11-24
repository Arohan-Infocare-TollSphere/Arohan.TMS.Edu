using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Arohan.TMS.Infrastructure.Persistence.DesignTime
{
    /// <summary>
    /// Design-time factory for EF Tools. It creates ApplicationDbContext with a connection
    /// string obtained from environment variable "TMS_CONNECTION" or appsettings.json.
    /// It passes a null tenant provider (migrations shouldn't rely on tenant).
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            // Try environment variable first (recommended)
            var connectionString = Environment.GetEnvironmentVariable("TMS_CONNECTION")
                               ?? config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string not found. Set TMS_CONNECTION environment variable or add DefaultConnection in appsettings.json.");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure());

            // Note: passing null for TenantProvider here; migrations should not rely on tenant filter.
            return new ApplicationDbContext(optionsBuilder.Options, null);
        }
    }
}
