using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationInfoContext : DbContext
    {
        public DbSet<ApplicationInfo> ApplicationsInfos { get; set; }
        public DbSet<ClientComputer> ClientComputers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=TTN;Database=ateatt;User Id=sa;Password=root;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}