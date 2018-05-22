using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBot.Domain
{
    public class Word : Entity, IEquatable<string>
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
            Occurrance = 1;
        }

        public bool Equals(string other)
        {
            return Value == other;
        }
    }
}