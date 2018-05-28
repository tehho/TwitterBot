using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBot.Domain
{
    public class ProfileOccurrance
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public int BotOptionsId{ get; set; }
        public BotOptions BotOptions { get; set; }
        public int Occurrance { get; set; }
        
        public ProfileOccurrance()
        {
        }

        public ProfileOccurrance(Profile profile, BotOptions options)
        {
            Profile = profile;
            BotOptions = options;
            Occurrance = 1;
        }
    }
}