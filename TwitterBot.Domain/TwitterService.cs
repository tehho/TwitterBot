using Tweetinvi;

namespace TwitterBot.Domain
{
    public class TwitterService
    {
        private readonly TwitterProfile1 _profile;
        public TwitterService(TwitterProfile1 profile, Token customer, Token access)
        {
            _profile = profile;

            Auth.SetUserCredentials(customer.Key, customer.Secret, access.Key, access.Secret);



        }

        
    }
}