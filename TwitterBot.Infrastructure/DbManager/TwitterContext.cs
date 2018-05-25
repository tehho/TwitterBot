using Microsoft.EntityFrameworkCore;
using TwitterBot.Domain;

namespace TwitterBot.Infrastructure
{
    public class TwitterContext : DbContext
    {
        public DbSet<TwitterProfile> TwitterProfiles { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordContainer> Containers { get; set; }
        public DbSet<BotOption> BotOptions { get; set; }

        public TwitterContext(DbContextOptions<TwitterContext> context) : base(context)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfileOccurrance>()
                .HasKey(pocc => new {pocc.ProfileId, pocc.BotOptionId});

            modelBuilder.Entity<ProfileOccurrance>()
                .HasOne(pocc => pocc.BotOption)
                .WithMany(botOption => botOption.ProfileOccurances)
                .HasForeignKey(profile => profile.ProfileId);



            modelBuilder.Entity<WordContainerOccurrence>()
                .HasKey(wco => new {wco.WordId, wco.ProfileId});

            modelBuilder.Entity<WordContainerOccurrence>()
                .HasOne(wco => wco.Profile)
                .WithMany(profile => profile.WordList)
                .HasForeignKey(wco => wco.ProfileId);

            

            modelBuilder.Entity<WordOccurrance>()
                .HasKey(wo => new {wo.WordId, wo.WordContainerId});

            modelBuilder.Entity<WordOccurrance>()
                .HasOne(wo => wo.WordContainer)
                .WithMany(wc => wc.Occurrances)
                .HasForeignKey(w => w.WordId);

            modelBuilder.Entity<BotOption>()
                .HasMany(option => option.ProfileOccurances)
                .WithOne(bot => bot.BotOption);

            base.OnModelCreating(modelBuilder);
        }

    }
}
