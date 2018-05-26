using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            //Console.WriteLine(BotTest.Test());

            Console.WriteLine("Post a tweet \n");



            Write("Check user name: ");

            string input = Console.ReadLine();

            TwitterProfile profile = new TwitterProfile()
            {
                Name = input
            };

            var tweetService = new TwitterService(new TwitterServiceOptions());

            var result = tweetService.DoesTwitterUserExist(profile);

            Console.WriteLine(result);

  

            ReadKey();

        }

        public static void PostATweet()
        {
            Console.WriteLine("Post a tweet \n");


            Write("Twitter post: ");

            var message = ReadLine().Trim();

            TwitterBot.Domain.Tweet userPost = new TwitterBot.Domain.Tweet
            {
                Text = message,
            };

            Console.WriteLine(userPost.Text);

            var tweetService = new TwitterService(new TwitterServiceOptions());

            tweetService.PublishTweet(userPost);

            Console.WriteLine("Tweet was published.");
        }

        public static void UpdateProfileImage(TwitterProfile profile)
        {

            var tweetService = new TwitterService(new TwitterServiceOptions());

            Write("Uploading image...");

            tweetService.UpdateProfileImage(tweetService.SaveProfileImageToServer(profile));

            Console.WriteLine("Image was updated.");
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
