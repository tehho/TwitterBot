using TwitterBot.Domain;

namespace TwitterBot.Api.Model
{
    public class TweetApi
    {
        public string Text { get; set; }

        public static implicit operator Tweet(TweetApi value)
        {
            return new Tweet { Text = value.Text };
        }
    }
}