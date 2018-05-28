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
        private readonly IRepository<WordOccurrence> _wordOccurranceRepository;
        public TwitterProfileTrainer(IRepository<Word> wordRepository, IRepository<WordOccurrence> wordOccurranceRepository)
        {
            _wordRepository = wordRepository;
            _wordOccurranceRepository = wordOccurranceRepository;
        }

        public TwitterProfile Train(TwitterProfile profile, TextContent content)
        {
            var regex = new Regex(@"(\.|,| |!|\?)");
            var words = regex.Split(content.Text).Where(word => !string.IsNullOrWhiteSpace(word)).Select(word => new Word(word)).ToList();

            WordOccurrence lastWordOccurrence = null;

            foreach (var word in words)
            {
                var tempWord = profile.Vocabulary.SingleOrDefault(w => w.Equals(word)) ??
                               (_wordRepository.Get(word) ??
                               (_wordRepository.Add(word) ??
                               throw new InvalidOperationException()));
                
                var currentWordOccurrence = profile.Words.SingleOrDefault(wo => wo.Word.Equals(tempWord));

                if (currentWordOccurrence == null)
                {
                    currentWordOccurrence = new WordOccurrence(tempWord, profile);
                    profile.AddOccurrence(tempWord);
                }
                else
                {
                    currentWordOccurrence.Occurrence++;
                }

                lastWordOccurrence?.AddOccurrence(currentWordOccurrence);

                lastWordOccurrence = currentWordOccurrence;
            }

            return profile;

        }
    }
}