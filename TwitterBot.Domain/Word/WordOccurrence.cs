using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class WordOccurrence : Entity
    {
        public int WordId { get; set; }
        public Word Word { get; set; }
        public int TwitterProfileId { get; set; }
        public TwitterProfile TwitterProfile { get; set; }
        public List<NextWordOccurrence> NextWordOccurrences { get; set; }
        public int Occurrence { get; set; }
        public List<WordOccurrence> NextWords => NextWordOccurrences.Select(nwo => nwo.Word).ToList();

        public WordOccurrence()
        {
            NextWordOccurrences = new List<NextWordOccurrence>();
        }

        public WordOccurrence(Word word, TwitterProfile profile)
        {
            Word = word;
            WordId = word.Id.Value;

            TwitterProfile = profile;
            TwitterProfileId = profile.Id.Value;

            NextWordOccurrences = new List<NextWordOccurrence>(); 
            Occurrence = 1;
        }

        public void AddOccurrance(WordOccurrence other)
        {
            var occurrance = NextWordOccurrences.SingleOrDefault(nwo => nwo.FollowedById == other.Id);
            if (occurrance == null)
            {
                occurrance = new NextWordOccurrence(this, other);
            }
            else
            {
                occurrance.Occurrence++;
            }
        }
    }
}

