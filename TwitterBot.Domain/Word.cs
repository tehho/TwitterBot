using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterBot.Domain
{
    public class Word : Entity, IEquatable<string>, IEquatable<Word>
    {
        public string Value { get; }
        public int Occurrance { get; set; }
        public List<Word> NextWord { get; private set; }
        public Dictionary<string, int> AlternativeSpellings { get; set; }

        public Word(string word)
        {
            Value = word.ToLower();
            Occurrance = 1;
        }

        public void AddNextWord(Word word)
        {
            if (NextWord == null)
                NextWord = new List<Word>();

            if (NextWord.Any(w => w.Equals(word)))
                NextWord.Single(w => w.Equals(word)).Occurrance++;

            else
                NextWord.Add(word);
        }

        public bool Equals(string other) => Value == other;

        public bool Equals(Word other) => Value == other.Value.ToLower();
    }
}