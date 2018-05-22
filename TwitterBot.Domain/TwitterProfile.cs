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

            foreach (var word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                if (Words.Any(w => w.Equals(word)))
                    Words.SingleOrDefault(w => w.Equals(word)).Occurrance++;

                else
                    Words.Add(new Word(word));
            }
        }
    }
}
