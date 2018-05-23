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
            var words = regex.Split(content.Text).Select(word => new Word(word)).ToList();
            WordOccurrence lastWordOccurrence = null;

            foreach (var word in words)
            {
                if (string.IsNullOrWhiteSpace(word.Value))
                    continue;

                WordOccurrence currentWordOccurrence = Words.SingleOrDefault(w => w.Word.Equals(word));

                if (currentWordOccurrence == null)
                {
                    currentWordOccurrence = new WordOccurrence(word);
                    Words.Add(currentWordOccurrence);
                }
                else
                {
                    currentWordOccurrence.Occurrence++;
                }

                lastWordOccurrence?.Word.AddNextWord(word);

                lastWordOccurrence = currentWordOccurrence;
            }
        }
    }
}