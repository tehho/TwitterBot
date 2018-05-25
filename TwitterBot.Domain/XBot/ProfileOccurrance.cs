using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBot.Domain
{
    public class ProfileOccurrance
    {
        public int ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual TwitterProfile Profile { get; set; }

        public int BotOptionId{ get; set; }
        [ForeignKey(nameof(BotOptionId))]
        public virtual BotOption BotOption { get; set; }

        public int Occurrance { get; set; }
    }
}