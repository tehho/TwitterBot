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
        private readonly IRepository<TwitterProfile> _profileRepository;
        private readonly IRepository<Word> _wordRepository;

        private readonly TwitterService trainer;

        public TwitterProfileTrainer(IRepository<TwitterProfile> profileRepository, IRepository<Word> wordRepository)
        {
            _profileRepository = profileRepository;
            _wordRepository = wordRepository;
        }

        public void Train(TwitterProfile profile, TextContent content)
        {
            var regex = new Regex(@"(\.|,| |!|\?)");
            var words = regex.Split(content.Text).Select(word => new Word(word)).ToList();

            Word parent = null;

            foreach (var word in words)
            {
                var tempWord = profile.Vocabulary.SingleOrDefault(w => w.Value == word.Value);

                if (tempWord == null)
                {
                    tempWord = _wordRepository.Get(word) ?? _wordRepository.Add(word);

                    profile.AddWord(tempWord);
                }
                else
                {

                }

                parent?.AddNextWord(tempWord);
                
                parent = tempWord;
            }

            _profileRepository.Update(profile);
        }
    }
}