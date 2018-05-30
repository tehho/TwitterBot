using System.ComponentModel.DataAnnotations;
using TwitterBot.Api.Model;

namespace TwitterBot.Api.Model
{
    public class TwitterTrainDataApi
    {
        [Required]
        public TwitterProfileApi profile { get; set; }
        [Required]
        public TweetApi tweet { get; set; }
    }
}