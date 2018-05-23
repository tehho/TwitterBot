namespace TwitterBot.Domain
{
    public class TwitterServiceOptions
    {
        public int TweetCount { get; set; }
        public Token Customer { get; set; }
        public Token Access { get; set; }
    }
}