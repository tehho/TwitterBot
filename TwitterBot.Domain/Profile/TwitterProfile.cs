using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class TwitterProfile : Profile, IProfile
    {
        public TwitterProfile()
        {
            Name = "";
            Words = new List<WordOccurrence>();
        }

        public TwitterProfile(string name)
        {
            Name = name;
            Words = new List<WordOccurrence>();
        }

        public WordOccurrence AddOccurrence(Word word)
        {
            var wordOccurrence = new WordOccurrence(word, this);
            Words.Add(wordOccurrence);

            return wordOccurrence;
        }
    }
}