using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class TwitterProfile : Profile, ITrainableFromText
    {
        public TwitterProfile(string name) : base(name)
        {
        }

        public void TrainFromText(TextContent content)
        {
            var regex = new Regex(@"(.| |!|\?)");

            var words = regex.Split(content.Text).ToList();

            foreach (var word in words)
            {
                if (Words.Any(w => w.Equals(word)))
                    Words.SingleOrDefault(w => w.Equals(word)).Occurrance++;
                else
                    Words.Add(new Word(word));
            }
        }
    }
}
