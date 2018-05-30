using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;

namespace TwitterBot.Domain
{
    public class Tweet : TextContent
    {
        public long TwitterId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public TwitterProfile User { get; }
        public int QuoteCount { get; }
        public int ReplyCount { get; }
        public int RetweetCount { get; set; }
        public int FavoriteCount { get; set; }

        public Tweet()
        {
        }

        public Tweet(string text)
        {
            Text = text;
        }

        public Tweet(long twitterId, string text, DateTime createdAt, int userId, TwitterProfile user, int quoteCount, int replyCount, int retweetCount, int favoriteCount)
        {
            TwitterId = twitterId;
            Text = text;
            CreatedAt = createdAt;
            UserId = userId;
            User = user;
            QuoteCount = quoteCount;
            ReplyCount = replyCount;
            RetweetCount = retweetCount;
            FavoriteCount = favoriteCount;
        }

        public static Tweet Parse(ITweet tweet)
        {
            return new Tweet
            {
                Text = tweet.FullText,

                CreatedAt = tweet.CreatedAt,
                TwitterId = tweet.CreatedBy.Id,
                FavoriteCount = tweet.FavoriteCount,
                RetweetCount = tweet.RetweetCount
            };
        }
    }
}