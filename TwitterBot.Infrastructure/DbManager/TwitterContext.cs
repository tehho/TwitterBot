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
    }
}
