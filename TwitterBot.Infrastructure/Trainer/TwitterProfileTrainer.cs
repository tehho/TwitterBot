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
            var regexExtended = new Regex("(^@|^ $|^http|^HTTP)");
            var words = regex.Split(content.Text)
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Where(word => !regexExtended.IsMatch(word))
                .Select(word => new Word(word)).ToList();

            WordOccurrence lastWordOccurrence = null;
            Word tempWord = null;
            WordOccurrence currentWordOccurrence = null;

            foreach (var word in words)
            {
                    if (!_wordRepository.GetAll().Any(w => w.Equals(word)))
                        tempWord = _wordRepository.Add(word);

                    currentWordOccurrence = profile.Words.SingleOrDefault(wo => wo.Word.Id == tempWord.Id);

                    if (currentWordOccurrence == null)
                    {
                        currentWordOccurrence = profile.AddOccurrence(tempWord);
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