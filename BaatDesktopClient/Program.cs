﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using static System.Console;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using JsonSerializer = Tweetinvi.JsonSerializer;

namespace BaatDesktopClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("...");

            Auth.SetUserCredentials("GjMrzt4a9YJqKXRTNKjLN2CVi", 
                                    "w3koS8pDXMxDscBZnT7VFgGFeoNgv0qxgUa5YYcvrv2WoysfRD", 
                                    "998554298735845382-cHyJyzufzzSUzceD79y8zb0IkbfrPxi", 
                                    "B72OlpxIme0yz3ZHRVw0mCMDxKukXTcNuOvhD9d0ySCX8");

            var smallTweetCount = 10;
            var mediumTweetCount = 200;
            var largetweetCount = 3200;

            var userHandle = "realdonaldtrump"; //User.GetAuthenticatedUser();

            var tweeter = Tweetinvi.User.GetUserFromScreenName(userHandle).UserIdentifier.ToString();

            //Write("Vad vill du tweeta? : ");

            //Tweet.PublishTweet(ReadLine());

            Console.WriteLine(tweeter);

            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            // long userId = 25073877;

            var lastTweets = Timeline.GetUserTimeline(tweeter, smallTweetCount).ToArray();

            var allTweets = new List<ITweet>(lastTweets);
            var beforeLast = allTweets;

            while (lastTweets.Length > 0 && allTweets.Count <= smallTweetCount)
            {
                var idOfOldestTweet = lastTweets.Select(x => x.Id).Min();
                Console.WriteLine($"Oldest Tweet Id = {idOfOldestTweet}");

                var numberOfTweetsToRetrieve = allTweets.Count > 3000 ? 3200 - allTweets.Count : smallTweetCount;
                var timelineRequestParameters = new UserTimelineParameters
                {
                    // MaxId ensures that we only get tweets that have been posted 
                    // BEFORE the oldest tweet we received

                    MaxId = idOfOldestTweet - 1,
                    MaximumNumberOfTweetsToRetrieve = numberOfTweetsToRetrieve
                };

                lastTweets = Timeline.GetUserTimeline(tweeter, timelineRequestParameters).ToArray();
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

            WriteLine("Filen skrevs till disk");
            ReadKey();

        }
    }
}
