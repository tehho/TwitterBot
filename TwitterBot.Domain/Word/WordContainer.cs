using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TwitterBot.Domain
{
    public class WordContainer : Entity
    {
        [Required]
        public int WordId { get; set; }

        [ForeignKey(nameof(WordId))]
        public virtual Word Word { get; set; }

        public ICollection<WordOccurrance> Occurrances { get; set; }

        public List<Word> NextWords => Occurrances.Select(occ => occ.Word).ToList();

        public WordContainer()
        {
        }

        public WordContainer(Word word)
        {
            Word = word;
            Occurrances = new List<WordOccurrance>();
        }

        public Word AddWord(Word word)
        {
            if (word == null)
                throw new NullReferenceException();

            if (word.Value == null)
                throw new ArgumentNullException();

            if (Occurrances == null)
                Occurrances = new List<WordOccurrance>();

            var occurrance = Occurrances.SingleOrDefault(occ => occ.Word.Equals(word));

            if (occurrance == null)
                Occurrances.Add(new WordOccurrance(word, this));
            else
                occurrance.Occurrance++;

            return word;
        }
    }
}