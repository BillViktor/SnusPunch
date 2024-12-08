using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Shared.Models.Statistics;

namespace SnusPunch.Data.DbContexts
{
    public class SnusPunchDbContext : IdentityDbContext<SnusPunchUserModel>
    {
        public DbSet<SnusModel> Snus { get; set; }
        public DbSet<EntryModel> Entries { get; set; }
        public DbSet<EntryLikeModel> EntryLikes { get; set; }
        public DbSet<EntryCommentModel> EntryComments { get; set; }
        public DbSet<StatisticsTimePeriodResponseDto> StatisticsTimePeriod { get; set; }
        public DbSet<SnusPunchFriendRequestModel> SnusPunchFriendRequests { get; set; }
        public DbSet<SnusPunchFriendModel> SnusPunchFriends { get; set; }
        public DbSet<EntryCommentLikeModel> EntryCommentLikes { get; set; }

        public SnusPunchDbContext(DbContextOptions<SnusPunchDbContext> aDbContextOptions) : base(aDbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder aModelBuilder)
        {
            base.OnModelCreating(aModelBuilder);

            //No table, used in a query
            aModelBuilder.Entity<StatisticsTimePeriodResponseDto>(e =>
            {
                e.HasNoKey();
                e.Metadata.SetIsTableExcludedFromMigrations(true);

                e.Property(p => p.AvgCostPerDayInSek).HasColumnType("decimal(6,2)");

                e.Property(p => p.TotalCostInSek).HasColumnType("decimal(6,2)");
            });

            aModelBuilder.Entity<SnusPunchFriendModel>(e =>
            {
                e.ToTable("tblSnusPunchUserFriend");

                e.HasKey(e => new
                {
                    e.SnusPunchUserModelOneId,
                    e.SnusPunchUserModelTwoId
                });

                e.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

                e.HasOne(e => e.SnusPunchUserModelOne)
                    .WithMany(e => e.FriendsAddedByUser)
                        .HasForeignKey(e => e.SnusPunchUserModelOneId)
                            .OnDelete(DeleteBehavior.ClientCascade);

                e.HasOne(e => e.SnusPunchUserModelTwo)
                    .WithMany(e => e.FriendsAddedByOthers)
                        .HasForeignKey(e => e.SnusPunchUserModelTwoId)
                            .OnDelete(DeleteBehavior.ClientCascade);
            });

            aModelBuilder.Entity<SnusPunchFriendRequestModel>(e =>
            {
                e.ToTable("tblSnusPunchUserFriendRequest");

                e.HasKey(e => new
                {
                    e.SnusPunchUserModelOneId,
                    e.SnusPunchUserModelTwoId
                });

                e.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

                e.HasOne(e => e.SnusPunchUserModelOne)
                    .WithMany()
                        .HasForeignKey(e => e.SnusPunchUserModelOneId)
                            .OnDelete(DeleteBehavior.ClientCascade);

                e.HasOne(e => e.SnusPunchUserModelTwo)
                    .WithMany()
                        .HasForeignKey(e => e.SnusPunchUserModelTwoId)
                            .OnDelete(DeleteBehavior.ClientCascade);
            });

            aModelBuilder.Entity<EntryCommentModel>(e =>
            {
                e.ToTable("tblEntryComment");
                e.HasKey(e => e.Id);

                e.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

                e.HasOne(e => e.EntryModel)
                    .WithMany(e => e.Comments)
                        .HasForeignKey(e => e.EntryId)
                            .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(e => e.ParentComment)
                    .WithMany(p => p.Replies)
                        .HasForeignKey(e => e.ParentCommentId)
                            .IsRequired(false)
                                .OnDelete(DeleteBehavior.ClientCascade);
            });

            aModelBuilder.Entity<EntryCommentLikeModel>(e =>
            {
                e.ToTable("tblEntryCommentLike");

                e.HasKey(e => e.Id);

                //Composite key
                e.HasIndex(c => new
                {
                    c.EntryCommentId,
                    c.SnusPunchUserModelId
                }).IsUnique();

                e.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

                e.HasOne(e => e.EntryCommentModel)
                    .WithMany(e => e.CommentLikes)
                        .HasForeignKey(e => e.EntryCommentId)
                            .OnDelete(DeleteBehavior.Cascade);
            });

            aModelBuilder.Entity<EntryLikeModel>(e =>
            {
                e.ToTable("tblEntryLike");

                e.HasKey(e => e.Id);

                //Composite key
                e.HasIndex(c => new
                {
                    c.EntryId,
                    c.SnusPunchUserModelId
                }).IsUnique();

                e.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

                e.HasOne(e => e.EntryModel)
                    .WithMany(e => e.Likes)
                        .HasForeignKey(e => e.EntryId)
                            .OnDelete(DeleteBehavior.Cascade);
            });

            aModelBuilder.Entity<EntryModel>(e =>
            {
                e.ToTable("tblEntry", tb =>
                {
                    tb.HasTrigger("TR_tblEntry_Insert");
                    tb.HasTrigger("TR_tblEntry_Update");
                });

                e.Property(p => p.SnusPortionPriceInSek).HasColumnType("decimal(6,2)");

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
                e.Property(c => c.CreateDate).HasDefaultValueSql("getdate()");

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
