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
        public IReadOnlyList<Word> Vocabulary => Words?.Select(wo => wo.Word).ToList();

        public List<WordOccurrence> Words { get; set; }

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

        public WordOccurrence AddOccurrence(Word tempWord)
        {
            var ret = new WordOccurrence(tempWord, this);
            Words.Add(ret);
            return ret;
        }
    }
}