using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;

namespace TwitterBot.Domain
{
    public class Tweet : TextContent
    {
        public long TwitterId { get; }
        public DateTime CreatedAt { get; }
        public int UserId { get; }
        public TwitterProfile User { get; }
        public int QuoteCount { get; }
        public int ReplyCount { get; }
        public int RetweetCount { get; }
        public int FavoriteCount { get; }

        public Tweet()
        {
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
    }
}