using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class TwitterProfile : Entity, IProfile , ITrainableFromText
    {
        public string Name { get; set; }
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
            var words = regex.Split(content.Text).ToList();
            WordOccurrence lastWordOccurrence = null;

            foreach (var word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                WordOccurrence currentWordOccurrence;

                if (Words.Any(w => w.Word.Equals(word)))
                {
                    currentWordOccurrence = Words.Single(w => w.Word.Equals(word));
                    currentWordOccurrence.Occurrence++;
                }

                else
                {
                    currentWordOccurrence = new WordOccurrence(new Word(word));
                    Words.Add(currentWordOccurrence);
                }

                if (lastWordOccurrence != null)
                {
                    if (currentWordOccurrence.NextWords.Any(w => w.Word.Equals(word)))
                        currentWordOccurrence.NextWords.Single(n => n.Word.Word.Equals(word)).Occurrence++;

                    else
                        currentWordOccurrence.NextWords.Add(new NextWordOccurrence(lastWordOccurrence, currentWordOccurrence));
                }

                lastWordOccurrence = currentWordOccurrence;
            }
        }
    }
}