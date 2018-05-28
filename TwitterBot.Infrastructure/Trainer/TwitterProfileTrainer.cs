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
                try
                {
                    tempWord = profile.Vocabulary.SingleOrDefault(w => w.Equals(word)) ??
                                   (_wordRepository.Get(word) ??
                                    _wordRepository.Add(word));

                    if (tempWord == null)
                        continue;

                    currentWordOccurrence = profile.Words.SingleOrDefault(wo => wo.Word.Id == tempWord.Id);

                    if (currentWordOccurrence == null)
                    {
                        currentWordOccurrence = profile.AddOccurrence(tempWord);
                    }
                    else
                    {
                        currentWordOccurrence.Occurrence++;
                    }

                    lastWordOccurrence?.AddOccurrance(currentWordOccurrence);

                    lastWordOccurrence = currentWordOccurrence;
                }
                catch (Exception e)
                {
                    throw (e);
                }
            }

            return profile;

        }
    }
}