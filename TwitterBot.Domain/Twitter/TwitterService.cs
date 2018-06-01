using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LinqToTwitter;
using Tweetinvi;
using Tweetinvi.Core.Exceptions;
using TwitterBot.Infrastructure.Logging;
using User = Tweetinvi.User;

namespace TwitterBot.Domain
{
    public class TwitterService
    {

        public int tweetCount;

        private readonly ILogger _logger;
        private readonly TwitterContext _context;

        // private readonly TwitterProfile _profile;
        //public TwitterService(TwitterProfile profile, Token customer, Token access)
        //{
        //    _profile = profile;
        //    tweetCount = 20;
        //    Auth.SetUserCredentials(customer.Key, customer.Secret, access.Key, access.Secret);
        //}

        public TwitterService(TwitterServiceOptions options, ILogger logger)
        {
            tweetCount = options.TweetCount;

            Auth.SetUserCredentials(options.Customer.Key, options.Customer.Secret,
                options.Access.Key, options.Access.Secret);

            var auth = new SingleUserAuthorizer()
            {
                CredentialStore = new SingleUserInMemoryCredentialStore()
                {
                    ConsumerKey = options.Customer.Key,
                    ConsumerSecret = options.Customer.Secret,
                    AccessToken = options.Access.Key,
                    AccessTokenSecret = options.Access.Secret
                }
            };

            _context = new TwitterContext(auth);

            _logger = logger;
        }

        public async Task<ITwitterException> IsTwitterUp()
        {
            if (! await DoesTwitterUserExist(new TwitterProfile("RowBoat2018")))
            {
                return ExceptionHandler.GetLastException();
            }
            else
                return null;
        }

        public async Task<bool> PublishTweet(Tweet tweet)
        {
            try
            {
                await _context.TweetAsync(tweet.Text);

                //Tweetinvi.Tweet.PublishTweet(tweet.Text);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;

        }

        public void UpdateProfileImage(byte[] image)
        {
            Tweetinvi.Account.UpdateProfileImage(image);
        }

        public byte[] SaveProfileImageToServer(TwitterProfile profile)
        {
            var profileImage = "https://twitter.com/" + profile.Name + "/profile_image?size=original";

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(profileImage), @"profile.jpg");
            }

            string path = @"profile.jpg";

            return File.ReadAllBytes(path);

        }

        public async Task<bool> DoesTwitterUserExist(TwitterProfile profile)
        {
            var user = (await GetTwitterProfile(profile.Name));

            return user != null;
        }

        public async Task<string> GetTwitterUserName(TwitterProfile profile)
        {
            return  (await GetTwitterProfile(profile.Name)).Name;
        }

        public bool ProfileTimeLineHasTweets(TwitterProfile profile)
        {
            if (profile == null)
                return false;

            if (profile.Name == null)
                return false;

            var user = User.GetUserFromScreenName(profile.Name);

            if (user == null)
                return false;

            var timelineParameter = Timeline.CreateHomeTimelineParameter();
            timelineParameter.ExcludeReplies = true;
            timelineParameter.TrimUser = true;
            timelineParameter.IncludeEntities = false;

            var timeLine = Timeline.GetUserTimeline(user);

            return timeLine != null;
        }

        public async Task<IEnumerable<Tweet>> GetAllTweetsFromProfile(TwitterProfile profile)
        {
            var tweets = await (from tweet in _context.Status
                where tweet.Type == StatusType.User &&
                      tweet.ScreenName == profile.Name
                select tweet).ToListAsync();

            return tweets.Select(tweet => new Tweet(tweet.Text));
        }

        private async Task<TwitterProfile> GetTwitterProfile(string name)
        {
            try
            {
                var user = await (from tweet in _context.User
                    where tweet.Type == UserType.Show &&
                          tweet.ScreenName == name
                    select tweet).SingleOrDefaultAsync();

                return user == null ? null : new TwitterProfile(user.ScreenName);
            }
            catch (Exception e)
            {
                _logger.Log(e.ToString());
                return null;
            }

        }
    }
}