using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Data.DbContexts
{
    public class SnusPunchDbContext : IdentityDbContext<SnusPunchUserModel>
    {
        public DbSet<SnusModel> Snus { get; set; }
        public DbSet<EntryModel> Entries { get; set; }

        public SnusPunchDbContext(DbContextOptions<SnusPunchDbContext> aDbContextOptions) : base(aDbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder aModelBuilder)
        {
            base.OnModelCreating(aModelBuilder);

            aModelBuilder.Entity<EntryModel>(e =>
            {
                e.ToTable("tblEntry", tb =>
                {
                    tb.HasTrigger("TR_tblEntry_Update");
                });

                e.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

                e.HasOne(e => e.Snus)
                    .WithMany()
                        .HasForeignKey(e => e.SnusId)
                            .OnDelete(DeleteBehavior.SetNull);

                e.HasOne(e => e.SnusPunchUserModel)
                    .WithMany(e => e.Entries)
                        .HasForeignKey(e => e.SnusPunchUserModelId)
                            .OnDelete(DeleteBehavior.Cascade);
            });

            aModelBuilder.Entity<SnusPunchUserModel>(e =>
            {
                e.HasOne(e => e.FavoriteSnus)
                    .WithMany()
                        .HasForeignKey(e => e.FavoriteSnusId)
                            .OnDelete(DeleteBehavior.SetNull);
            });


            aModelBuilder.Entity<SnusModel>(e =>
            {
                e.ToTable("tblSnus", tb =>
                {
                    tb.HasTrigger("TR_tblSnus_Update");
                });

                e.Property(p => p.PriceInSek).HasColumnType("decimal(6,2)");

                e.Property(p => p.PricePerPortion).HasComputedColumnSql("PricePerPortion").HasColumnType("decimal(6,2)");
                e.Property(p => p.NicotinePerPortion).HasComputedColumnSql("NicotinePerPortion");

                e.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");
            });
        }
    }
}
