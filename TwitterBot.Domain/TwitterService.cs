using System.Collections.Generic;
using System.Linq;
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

        public List<ITweet> ListAllTweetsFromProfile(TwitterProfile profile)
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

            return allTweets;


        }


    }
}