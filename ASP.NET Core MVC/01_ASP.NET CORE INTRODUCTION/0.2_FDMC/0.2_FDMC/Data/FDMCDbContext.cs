using Microsoft.EntityFrameworkCore;

namespace _0._2_FDMC.Data
{
    public class FDMCDbContext : DbContext
    {
        public FDMCDbContext(DbContextOptions<FDMCDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cat> Cats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
