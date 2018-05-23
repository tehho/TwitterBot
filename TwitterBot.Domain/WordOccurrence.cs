using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class WordOccurrence : Entity
    {
        [Required]
        public int? WordId { get; set; }
        [Required]
        public Word Word { get; set; }
        [Required]
        public int Occurrence { get; set; }

        public WordOccurrence()
        {
            Word = null;
            WordId = null;
            Occurrence = 0;
        }

        public WordOccurrence(Word word)
        {
            Word = word;
            Occurrence = 1;
        }
    }
}
