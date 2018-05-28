using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBot.Domain
{
    public class ProfileOccurrance
    {
        public int ProfileId { get; set; }
        public virtual IProfile Profile { get; set; }

        public int BotOptionsId{ get; set; }
        public virtual BotOptions BotOptions { get; set; }

        public int Occurrance { get; set; }


        public ProfileOccurrance()
        {
            
        }

        public ProfileOccurrance((IProfile) profile, BotOptions options)
        {
            Profile = profile;
            ProfileId = profile.Id.Value;

            BotOptions = options;
            BotOptionsId = options.Id.Value;
            
            Occurrance = 1;
        }
    }
}