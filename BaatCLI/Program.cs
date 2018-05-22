using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using TwitterBot.Domain;
using static System.Console;

namespace BaatDesktopClient
{
    class Program
    {
        public const int smallTweetCount = 10;
        public const int mediumTweetCount = 200;
        public const int largeTweetCount = 3200;

        static void Main(string[] args)
        {
            Console.WriteLine(BotTest.Test());

            //Console.WriteLine("Post a tweet \n");

            //var tweetService = new TwitterService(null
            //    , new Token
            //    {
            //        Key = "GjMrzt4a9YJqKXRTNKjLN2CVi",
            //        Secret = "w3koS8pDXMxDscBZnT7VFgGFeoNgv0qxgUa5YYcvrv2WoysfRD"
            //    },
            //    new Token()
            //    {
            //        Key = "998554298735845382-cHyJyzufzzSUzceD79y8zb0IkbfrPxi",
            //        Secret = "B72OlpxIme0yz3ZHRVw0mCMDxKukXTcNuOvhD9d0ySCX8"
            //    });

            //Write("Twitter post: ");

            //var message = ReadLine().Trim();

            //TwitterBot.Domain.Tweet userPost = new TwitterBot.Domain.Tweet
            //{
            //    Text = message,
            //};

            //Console.WriteLine(userPost.Text);

            //tweetService.PublishTweet(userPost);

            //Console.WriteLine("Tweet was published.");

            //ReadKey();

        }

        public static void SaveAllTweetsFromProfileToJson(TwitterProfile profile)
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

            var jsonOfTweets = allTweets.Distinct().ToJson();
            string fileName = "test.json";


            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.CreateNew);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(jsonOfTweets);

                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }


        }

        public static List<ITweet> ListAllTweetsFromProfile(TwitterProfile profile)
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
