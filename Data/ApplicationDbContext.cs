using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChannelEngineConsoleUpgraded.Data;

namespace ChannelEngineAssignment.Data
{
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ChannelEngineConsoleUpgraded.Data.RankingProduct>? RankingProduct { get; set; }
    }
}