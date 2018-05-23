using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class TwitterProfile : Entity, IProfile, ITrainableFromText
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

        public void TrainFromText(TextContent content)
        {
            var regex = new Regex(@"(\.|,| |!|\?)");
            var words = regex.Split(content.Text).Select(word => new Word(word)).ToList();

            WordOccurrence lastWordOccurrence = null;

            Word lastWord = null;

            foreach (var word in words)
            {
                if (string.IsNullOrWhiteSpace(word.Value))
                    continue;

                WordOccurrence currentWordOccurrence;

                if (Words.Any(w => w.Word.Equals(word)))
                {
                    currentWordOccurrence = Words.Single(w => w.Word.Equals(word));
                    currentWordOccurrence.Occurrence++;
                }

                else
                {
                    currentWordOccurrence = new WordOccurrence(new Word(word), null);
                    Words.Add(currentWordOccurrence);
                }

                lastWordOccurrence?.Word.AddNextWordOccurrence(currentWordOccurrence);

                lastWordOccurrence = currentWordOccurrence;

                var test = Words.SingleOrDefault(wc => wc.Word.Equals(word));


                if (test == null)
                {
                    currentWordOccurrence = new WordOccurrence(word);
                }
            }
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