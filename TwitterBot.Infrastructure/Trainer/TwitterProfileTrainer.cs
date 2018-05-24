using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitterBot.Domain;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Infrastructure
{
    public class TwitterProfileTrainer
    {
        private readonly IRepository<Word> _wordRepository;
        private readonly IRepository<WordContainer> _containerRepository;

        public TwitterProfileTrainer(IRepository<Word> wordRepository, IRepository<WordContainer> containerRepository)
        {
            _wordRepository = wordRepository;
            _containerRepository = containerRepository;
        }

        public TwitterProfile Train(TwitterProfile profile, TextContent content)
        {
            var regex = new Regex(@"(\.|,| |!|\?)");
            var words = regex.Split(content.Text).Where(word => !string.IsNullOrWhiteSpace(word)).Select(word => new Word(word)).ToList();

            WordContainer parent = null;

            foreach (var word in words)
            {
                var tempWord = profile.Vocabulary.SingleOrDefault(w => w.Equals(word)) 
                               ?? (_wordRepository.Get(word) ?? _wordRepository.Add(word));

                var tempContainer = profile.Containers.SingleOrDefault(wc => wc.Word.Equals(word)) 
                                    ?? _containerRepository.Add(new WordContainer(word));

                profile.AddWordContainer(tempContainer);

                parent?.AddWord(tempWord);

                parent = tempContainer;

            }

            return profile;
        }
    }
}