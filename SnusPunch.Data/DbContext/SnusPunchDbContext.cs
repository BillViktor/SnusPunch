using Microsoft.EntityFrameworkCore;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Data.DbContexts
{
    public class SnusPunchDbContext : DbContext
    {
        public DbSet<SnusModel> Snus { get; set; }

        public SnusPunchDbContext(DbContextOptions<SnusPunchDbContext> aDbContextOptions) : base(aDbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder aModelBuilder)
        {
            base.OnModelCreating(aModelBuilder);

            aModelBuilder.Entity<SnusModel>(entity =>
            {
                entity.ToTable("Snus", tb =>
                {
                    tb.HasTrigger("TR_Snus_Insert");
                    tb.HasTrigger("TR_Snus_Update");
                });

                entity.Property(p => p.PriceInSek).HasColumnType("decimal(18,2)");
            });
        }
    }
}
