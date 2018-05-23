namespace TwitterBot.Domain
{
    public class ProfileWordOccurrence : WordOccurrence
    {
        public int ParentId { get; set; }
        public TwitterProfile Parent { get; set; }

        public ProfileWordOccurrence()
        {
            
        }

        public ProfileWordOccurrence(Word word, TwitterProfile parent) : base(word)
        {
            Parent = parent;
        }
    }
}