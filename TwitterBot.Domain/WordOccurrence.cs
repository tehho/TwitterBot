using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class WordOccurrence : Entity
    {
        public int WordId { get; set; }
        public Word Word { get; set; }
        public int Occurrence { get; set; }

        public WordOccurrence(Word word)
        {
            Word = word;
            Occurrence = 1;
        }
    }
}
