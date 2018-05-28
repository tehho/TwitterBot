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
            modelBuilder.Entity<BotOptions>()
                .HasMany(b => b.Profiles)
                .WithOne(p => p.BotOptions);

            modelBuilder.Entity<ProfileOccurrance>()
                .HasOne(p => p.Profile);

            modelBuilder.Entity<WordOccurrence>()
                .HasOne(w => w.Word);

            modelBuilder.Entity<WordOccurrence>()
                .HasMany(w => w.NextWordOccurrences)
                .WithOne(n => n.Word)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NextWordOccurrence>()
                .HasOne(w => w.FollowedBy);

            base.OnModelCreating(modelBuilder);
        }

    }
}
