using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TwitterBot.Domain
{
    public class Word : Entity, IEquatable<string>, IEquatable<Word>
    {
        public string Value { get; set; }

        public Word()
        {
            Value = "";
        }

        public Word(string word)
        {
            Value = word.ToLower();
        }

        public bool Equals(string other) => Value == other.ToLower();

        public bool Equals(Word other) => Equals(other.Value);
    }
}