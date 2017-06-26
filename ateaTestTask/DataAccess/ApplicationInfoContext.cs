using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
    public class ApplicationInfoContext : DbContext
    {
        public DbSet<ApplicationInfo> ApplicationsInfos { get; set; }
        public DbSet<ClientComputer> ClientComputers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }


        public ApplicationInfoContext()
        {
        }
        public ApplicationInfoContext(DbContextOptions<ApplicationInfoContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");

            var basePath = AppContext.BaseDirectory;
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString("AppsInfoDb");
            optionsBuilder.UseSqlServer(connstr);
            base.OnConfiguring(optionsBuilder);
        }
    }
}