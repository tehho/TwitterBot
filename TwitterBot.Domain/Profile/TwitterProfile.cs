using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class TwitterProfile : Entity, IProfile
    {
        public string Name { get; set; }
        public IReadOnlyList<Word> Vocabulary => Words?.Select(w => w.Word).ToList();
        public IList<WordOccurrence> Words { get; set; }

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

        public void AddOccurrence(Word word)
        {
            Words.Add(new WordOccurrence(word, this));
        }
    }
}