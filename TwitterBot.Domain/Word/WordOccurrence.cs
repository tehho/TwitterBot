using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class WordOccurrence : Entity
    {
        [Required]
        public Guid WordId { get; set; }
        [Required]
        public Word Word { get; set; }
        [Required]
        public Guid TwitterProfileId { get; set; }
        [Required]
        public TwitterProfile TwitterProfile { get; set; }

        public virtual IList<NextWordOccurrence> NextWordOccurrences { get; set; }
        public int Occurrence { get; set; }

        public WordOccurrence()
        {
            NextWordOccurrences = new List<NextWordOccurrence>();
            Occurrence = 1;
        }

        public WordOccurrence(Word word, TwitterProfile profile)
        {
            // TODO: Null-checks

            Word = word;
            WordId = word.Id.Value;

            TwitterProfile = profile;
            TwitterProfileId = profile.Id.Value;

            NextWordOccurrences = new List<NextWordOccurrence>();
            Occurrence = 1;
        }

        public void AddOccurrence(Word other)
        {
            var occurrence = NextWordOccurrences.SingleOrDefault(nwo => nwo.Word.Id == other.Id);
            if (occurrence == null)
            {
                occurrence = new NextWordOccurrence(other, this);
                NextWordOccurrences.Add(occurrence);
            }
            else
            {
                occurrence.Occurrence++;
            }
        }
    }
}

