using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterBot.Domain
{
    public class TwitterService
    {
        public const int smallTweetCount = 10;
        public const int mediumTweetCount = 200;
        public const int largeTweetCount = 3200;

        private readonly TwitterProfile _profile;
        public TwitterService(TwitterProfile profile, Token customer, Token access)
        {
            _profile = profile;

            Auth.SetUserCredentials(customer.Key, customer.Secret, access.Key, access.Secret);



        }

        public void PublishTweet(Tweet tweet)
        {
            Tweetinvi.Tweet.PublishTweet(tweet.Text);
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

        public void UpdateProfileImage(byte[] image)
        {
            Tweetinvi.Account.UpdateProfileImage(image);
        }

        public IEnumerable<Tweet> ListAllTweetsFromProfile(TwitterProfile profile)
        {
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            var lastTweets = Timeline.GetUserTimeline(profile.Name, smallTweetCount).ToArray();

            var allTweets = new List<ITweet>(lastTweets);

            while (lastTweets.Length > 0 && allTweets.Count <= smallTweetCount)
            {
                var idOfOldestTweet = lastTweets.Select(x => x.Id).Min();

                var numberOfTweetsToRetrieve = allTweets.Count > 3000 ? 3200 - allTweets.Count : smallTweetCount;
                var timelineRequestParameters = new UserTimelineParameters
                {
                    MaxId = idOfOldestTweet - 1,
                    MaximumNumberOfTweetsToRetrieve = numberOfTweetsToRetrieve
                };

                lastTweets = Timeline.GetUserTimeline(profile.Name, timelineRequestParameters).ToArray();
                allTweets.AddRange(lastTweets);
            }

            return allTweets.Select(Tweet.Parse);


        }


    }
}