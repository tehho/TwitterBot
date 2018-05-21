using Tweetinvi;

namespace TwitterBot.Domain
{
    public class TwitterService
    {
        private readonly TwitterProfile _profile;
        public TwitterService(TwitterProfile profile, Token customer, Token access)
        {
            _profile = profile;

            Auth.SetUserCredentials(customer.key, customer.secret, access.key, access.secret);



        }

        
    }
}