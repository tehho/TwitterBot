using System.Collections.Generic;

namespace TwitterBot.Domain
{
    public class Word : Entity
    {
        public string Value { get; set; }
        public Dictionary<Word, int> NextWord { get; set; }
        public Dictionary<string, int> AlternativeSpellings { get; set; }
    }
}