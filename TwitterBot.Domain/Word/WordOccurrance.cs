using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBot.Domain
{
    public class WordOccurrance
    {
        [Required]
        public int WordId { get; set; }
        [ForeignKey(nameof(WordId))]
        public virtual Word Word { get; set; }

        [Required]
        public int WordContainerId { get; set; }
        [ForeignKey(nameof(WordContainerId))]
        public virtual WordContainer WordContainer { get; set; }
        [Required]
        public int Occurrance { get; set; }

        public WordOccurrance()
        {
            
        }

        public WordOccurrance(Word word, WordContainer container)
        {
            Occurrance = 1;
            Word = word;
            WordId = Word.Id.Value;
            WordContainer = container;
            WordContainerId = container.Id.Value;
        }

    }
}