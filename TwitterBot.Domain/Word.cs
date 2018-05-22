using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TwitterBot.Domain
{
    public class Word : Entity, IEquatable<string>, IEquatable<Word>
    {
        public string Value { get; set; }
        public List<WordOccurrence> NextWord { get; set; }
        //public Dictionary<string, int> AlternateSpellings { get; set; }

        public Word(string word)
        {
            Value = word.ToLower();
        }

        public void AddNextWordOccurrence(WordOccurrence wordOccurrence)
        {
            if (NextWord == null)
                NextWord = new List<WordOccurrence>();

            if (NextWord.Any(w => w.Equals(wordOccurrence.Word)))
                NextWord.Single(w => w.Equals(wordOccurrence.Word)).Occurrence++;

            else
                NextWord.Add(new WordOccurrence(wordOccurrence.Word));
        }

        public bool Equals(string other) => Value == other.ToLower();

        public bool Equals(Word other) => Value == other.Value.ToLower();
    }
}