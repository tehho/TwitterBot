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

        //[NotMapped]
        public IReadOnlyList<Word> Vocabulary => Words.Select(w => w.Word).ToList();

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

        public void AddWord(Word word)
        {
            if (word == null)
                throw new NullReferenceException();

            if (Words == null)
                Words = new List<WordOccurrence>();

            var nextWord = Words.SingleOrDefault(w => w.Word.Equals(word));

            if (nextWord == null)
                Words.Add(new ProfileWordOccurrence(word, this));
            else
                nextWord.Occurrence++;
        }
    }
}