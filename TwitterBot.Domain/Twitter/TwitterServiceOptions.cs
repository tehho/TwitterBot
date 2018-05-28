namespace TwitterBot.Domain
{
    public class TwitterServiceOptions
    {

        public int TweetCount { get; set; }
        public Token Customer { get; set; }
        public Token Access { get; set; }

        public TwitterServiceOptions(Appsettings Appsettings)
        {
            TweetCount = Appsettings.TweetCount;
            Customer = Appsettings.Customer;
            Access = Appsettings.Access;
        }
    }
}