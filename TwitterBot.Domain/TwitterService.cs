using Tweetinvi;

namespace TwitterBot.Domain
{
    public class TwitterService
    {
        private readonly TwitterProfile _profile;
        public TwitterService(TwitterProfile profile, Token customer, Token access)
        {
            _profile = profile;

            Auth.SetUserCredentials(customer.Key, customer.Secret, access.Key, access.Secret);



        }

        
    }
}