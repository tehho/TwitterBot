namespace TwitterBot.Domain
{
    public class WordWordOccurrence : WordOccurrence
    {
        public int ParentId { get; set; }
        public Word Parent { get; set; }

        public WordWordOccurrence()
        {
            
        }

        public WordWordOccurrence(Word word, Word parent) : base(word)
        {
            Parent = parent;
        }
    }
}