using System;
using System.Collections.Generic;

namespace TwitterBot.Domain
{
    public class Word : Entity, IEquatable<string>, IEquatable<Word>
    {
        public string Value { get; set; }
        public int Occurrance { get; set; }
        [NotMapped]
        public Dictionary<Word, int> NextWord { get; set; }
        [NotMapped]
        public Dictionary<string, int> AlternativeSpellings { get; set; }

        public Word(string word)
        {
            Value = word.ToLower();
        }

        public void AddNextWord(Word word)
        {
            if (NextWord == null)
                NextWord = new Dictionary<Word, int>();

            if (NextWord.ContainsKey(word))
                NextWord[word]++;

            else
                NextWord[word] = 1;
        }

        public bool Equals(string other) => Value == other;

        public bool Equals(Word other) => Value == other.Value.ToLower();
    }
}