using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class WordContainerOccurrence
    {
        [Required]
        public int WordId { get; set; }
        [ForeignKey(nameof(WordId))]
        public virtual WordContainer WordContainer { get; set; }

        [Required]
        public int Occurrence { get; set; }

        [Required]
        public int ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual TwitterProfile Profile { get; set; }

        public WordContainerOccurrence()
        {
        }

        public WordContainerOccurrence(WordContainer container, TwitterProfile profile)
        {
            WordContainer = container;
            WordId = container.Id.Value;
            Occurrence = 1;
            Profile = profile;
            ProfileId = profile.Id.Value;
        }
    }
}
