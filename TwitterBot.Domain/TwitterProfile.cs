using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class TwitterProfile : IProfile, ITrainableFromText
    {
        public string Name { get; set; }
        public List<Word> Words { get; set; }

        public TwitterProfile(string name)
        {
            Name = name;
            Words = new List<Word>();
        }

        public void TrainFromText(TextContent content)
        {
            var regex = new Regex(@"(.| |!|\?)");
            var words = regex.Split(content.Text).ToList();
            Word lastWord = null;

            foreach (var word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                Word currentWord;

                if (Words.Any(w => w.Equals(word)))
                {
                    currentWord = Words.Single(w => w.Equals(word));
                    currentWord.Occurrance++;
                }

                else
                {
                    currentWord = new Word(word);
                    Words.Add(currentWord);
                }

                if (lastWord != null)
                    lastWord.NextWord.Add(currentWord);
            }
        }
    }
}