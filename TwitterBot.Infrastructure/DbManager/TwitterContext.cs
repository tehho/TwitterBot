using System;
using TwitterBot.Domain;
using Microsoft.EntityFrameworkCore;

namespace TwitterBot.Infrastructure
{
    public class TwitterContext : DbContext
    {
        public DbSet<TwitterProfile> TwitterProfiles { get; set; }
        public DbSet<Word> Words { get; set; }

        public TwitterContext(DbContextOptions<TwitterContext> context) : base(context)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TwitterProfile>();
            modelBuilder.Entity<WordOccurrence>().HasOne(w => w.Word);
            modelBuilder.Entity<WordOccurrence>().HasMany(w => w.NextWords).WithOne(n => n.Word);
            modelBuilder.Entity<NextWordOccurrence>().HasOne(w => w.FollowedBy);
        }
    }
}
