using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnusPunch.Shared.Models.Identity;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Data.DbContexts
{
    public class SnusPunchDbContext : IdentityDbContext<SnusPunchUserModel>
    {
        public DbSet<SnusModel> Snus { get; set; }

        public SnusPunchDbContext(DbContextOptions<SnusPunchDbContext> aDbContextOptions) : base(aDbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder aModelBuilder)
        {
            base.OnModelCreating(aModelBuilder);

            aModelBuilder.Entity<SnusPunchUserModel>(entity =>
            {
                entity.HasOne(e => e.FavoriteSnus).WithMany().HasForeignKey(e => e.FavoriteSnusId).OnDelete(DeleteBehavior.SetNull);
            });

            aModelBuilder.Entity<SnusModel>(entity =>
            {
                entity.ToTable("Snus", tb =>
                {
                    tb.HasTrigger("TR_Snus_Insert");
                    tb.HasTrigger("TR_Snus_Update");
                });

                entity.Property(p => p.PriceInSek).HasColumnType("decimal(6,2)");

                entity.Property(p => p.PricePerPortion).HasComputedColumnSql("PricePerPortion").HasColumnType("decimal(6,2)");
                entity.Property(p => p.NicotinePerPortion).HasComputedColumnSql("NicotinePerPortion");

                entity.Property(c => c.CreateDate).HasDefaultValue(DateTime.Now);
            });
        }
    }
}
