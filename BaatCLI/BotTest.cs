using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TwitterBot.Domain;
using TwitterBot.Infrastructure;
using TwitterBot.Infrastructure.Repository;

namespace BaatDesktopClient
{
    public class BotTest
    {
        public static string Test()
        {
            var bot = new Bot("bot");
            var profile = new TwitterProfile("profile");
            var profileP = new TwitterProfile("profileP");
            var tweet1 = new Tweet { Text = @"Two years after an accident left @albertopajariyo in a wheelchair and unable to communicate, he returned to Twitter thanks to technology. Welcome back, Alberto!" };
            var tweet2 = new Tweet { Text = @"We recently found a bug that stored passwords unmasked in an internal log. We fixed the bug and have no indication of a breach or misuse by anyone. As a precaution, consider changing your password on all services where you’ve used this password." };
            var tweet3 = new Tweet { Text = @"We encourage you to update the app as we will no longer continue to support previous versions beginning on June 1. We’re doing this to focus on providing our Windows users with the latest and greatest, like night mode, which is coming soon." };

            profile.TrainFromText(tweet1);
            profile.TrainFromText(tweet2);
            profile.TrainFromText(tweet3);

            profileP.TrainFromText(tweet1);
            profileP.TrainFromText(tweet2);
            profileP.TrainFromText(tweet3);

            var opt = new DbContextOptionsBuilder<TwitterContext>();
            opt.UseSqlite(@"Filename=C:\Project\TwitterBot\TwitterBot.Infrastructure\DbManager\TwitterBot.db");
            opt.EnableSensitiveDataLogging();

            var conv = SqliteConventionSetBuilder.Build();
            var model = new ModelBuilder(conv);
            model.Entity<TwitterProfile>();
            model.Entity<NextWordOccurrence>().HasKey(w => new { w.WordId, w.FollowedById });
            opt.UseModel(model.Model);

            using (var context = new TwitterContext(opt.Options))
            {
                var repo = new TwitterProfileRepository(context);

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                repo.Add(profile);
                repo.Add(profileP);
            }

            using (var context = new TwitterContext(opt.Options))
            {
                var repo = new TwitterProfileRepository(context);

                var profile2 = repo.Get(new TwitterProfile {Name = "profile"});
                var profile22 = repo.Get(new TwitterProfile {Name = "profileP"});

                bot.AddProfile(profile22);

                return bot.GenerateRandomTweetText();
            }
        }
    }
}