namespace TwitterBot.Domain
{
    public class NextWordOccurrence : Entity
    {
        public int WordId { get; set; }
        public WordOccurrence Word { get; set; }
        public int FollowedById { get; set; }
        public WordOccurrence FollowedBy { get; set; }
        public int Occurrence { get; set; }

        public NextWordOccurrence()
        {
        }

        public NextWordOccurrence(WordOccurrence word, WordOccurrence followedBy)
        {
            Word = word;
            FollowedBy = followedBy;
            Occurrence = 1;
        }
    }
}
