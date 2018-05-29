using System;
using System.ComponentModel.DataAnnotations;

namespace TwitterBot.Domain
{
    public class NextWordOccurrence
    {
        public Guid WordId { get; set; }
        [Required]
        public Word Word { get; set; }
        public Guid ParentId { get; set; }
        [Required]
        public WordOccurrence Parent { get; set; }
        public int Occurrence { get; set; }

        public NextWordOccurrence()
        {
        }

        public NextWordOccurrence(Word word, WordOccurrence parent)
        {
            Word = word;

            Parent = parent;

            Occurrence = 1;
        }
    }
}
