using Microsoft.EntityFrameworkCore;
using TwitterBot.Domain;

namespace TwitterBot.Infrastructure
{
    public class TwitterContext : DbContext
    {
        public DbSet<TwitterProfile> TwitterProfiles { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordOccurrence> WordOccurrences { get; set; }
        public DbSet<BotOptions> BotOptions { get; set; }

        public TwitterContext(DbContextOptions<TwitterContext> context) : base(context)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TwitterProfile>();

            modelBuilder.Entity<BotOptions>()
                .HasMany(b => b.ProfileOccurances)
                .WithOne(p => p.BotOptions);

            modelBuilder.Entity<ProfileAlgorithmSelector>()
                .HasOne(a => a.BotOptions)
                .WithOne(b => b.ProfileAlgorithms)
                .HasForeignKey<BotOptions>(b => b.ProfileAlgorithmsId);

            modelBuilder.Entity<AlgorithmSelector>()
                .HasOne(a => a.BotOptions)
                .WithOne(b => b.WordAlgorithms)
                .HasForeignKey<BotOptions>(b => b.WordAlgorithmsId);

            modelBuilder.Entity<ProfileOccurrance>()
                .HasKey(occ => new {occ.BotOptionsId, occ.ProfileId});

            modelBuilder.Entity<ProfileOccurrance>()
                .HasOne(occ => occ.BotOptions)
                .WithMany(bo => bo.ProfileOccurances)
                //.HasForeignKey(occ => occ.BotOptionsId)
                ;

            modelBuilder.Entity<NextWordOccurrence>()
                .HasKey(nwo => new {nwo.WordId, nwo.ParentId});

            modelBuilder.Entity<NextWordOccurrence>()
                .HasOne(nwo => nwo.Parent)
                .WithMany(wo => wo.NextWordOccurrences)
                .HasForeignKey(nwo => nwo.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
