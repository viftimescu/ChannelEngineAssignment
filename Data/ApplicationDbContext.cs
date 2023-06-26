using ChannelEngineConsoleApp.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChannelEngineAssignment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Configure the Name of the RankingProduct to be its PK
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<RankingProduct>().HasKey(x => x.Name);
            base.OnModelCreating(modelBuilder);
        }

        // Configure the Name of the RankingProduct to be its PK
        public DbSet<ChannelEngineConsoleApp.Data.RankingProduct>? RankingProduct { get; set; }
    }
}