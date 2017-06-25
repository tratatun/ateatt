using Domain;
using Microsoft.EntityFrameworkCore;

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
    }
}