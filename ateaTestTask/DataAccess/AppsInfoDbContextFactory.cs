using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
    public class AppsInfoDbContextFactory : IDbContextFactory<ApplicationInfoContext>
    {
        public ApplicationInfoContext Create(DbContextFactoryOptions options)
        {
            return Create(options.ContentRootPath, options.EnvironmentName);
        }

        public ApplicationInfoContext Create()
        {
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");

            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName);
        }

        private ApplicationInfoContext Create(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString("AppsInfoDb");

            if (String.IsNullOrWhiteSpace(connstr) == true)
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named 'AppsInfoDb'.");
            }
            else
            {
                return Create(connstr);
            }
        }

        private ApplicationInfoContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(
                    $"{nameof(connectionString)} is null or empty.",
                    nameof(connectionString));

            var optionsBuilder =
                new DbContextOptionsBuilder<ApplicationInfoContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationInfoContext(optionsBuilder.Options);
        }

    }
}