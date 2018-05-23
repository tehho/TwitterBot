using System.Collections.Generic;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterBot.Domain
{
    public class TwitterService
    {

        public int tweetCount;

        private readonly TwitterProfile _profile;
        //public TwitterService(TwitterProfile profile, Token customer, Token access)
        //{
        //    _profile = profile;
        //    tweetCount = 20;
        //    Auth.SetUserCredentials(customer.Key, customer.Secret, access.Key, access.Secret);
        //}

        public TwitterService(TwitterServiceOptions options)
        {
            tweetCount = options.TweetCount;

            Auth.SetUserCredentials(options.Customer.Key, options.Customer.Secret, 
                options.Access.Key, options.Access.Secret);

        }

        public void PublishTweet(Tweet tweet)
        {
            Tweetinvi.Tweet.PublishTweet(tweet.Text);
        }

        public IEnumerable<Tweet> ListAllTweetsFromProfile(TwitterProfile profile)
        {
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            var lastTweets = Timeline.GetUserTimeline(profile.Name, tweetCount).ToArray();

            var allTweets = new List<ITweet>(lastTweets);

            while (lastTweets.Length > 0 && allTweets.Count <= tweetCount)
            {
                var idOfOldestTweet = lastTweets.Select(x => x.Id).Min();

                var numberOfTweetsToRetrieve = allTweets.Count > 3000 ? 3200 - allTweets.Count : tweetCount;
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