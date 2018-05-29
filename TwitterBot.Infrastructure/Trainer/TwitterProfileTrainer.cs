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
            if (content == null)
                return null;

            if (content.Text == null)
                return null;

            var regex = new Regex(@"(\.|,| |!|\?)");

            var regexExtended = new Regex("(^@|^ $|^http|^HTTP|^#)");

            var words = regex.Split(content.Text)
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Where(word => !regexExtended.IsMatch(word))
                .Select(word => new Word(word)).ToList();

            WordOccurrence lastWordOccurrence = null;
            Word tempWord = null;
            foreach (var word in words)
            {
                try
                {
                    if (word.Value == "be")
                        tempWord = null;

                    if (profile.Vocabulary.SingleOrDefault(w => w.Equals(word)) != null)
                        tempWord = profile.Vocabulary.SingleOrDefault(w => w.Equals(word));
                    else if (_wordRepository.Get(word) != null)
                        tempWord = _wordRepository.Get(word);
                    else
                        tempWord = _wordRepository.Add(word);

                    if (tempWord == null)
                        continue;

                    var currentWordOccurrence = profile.Words.SingleOrDefault(wo => wo.Word.Id == tempWord.Id);

                    if (currentWordOccurrence == null)
                    {
                        currentWordOccurrence = profile.AddOccurrence(tempWord);
                    }
                    else
                    {
                        currentWordOccurrence.Occurrence++;
                    }

                    lastWordOccurrence?.AddOccurrence(currentWordOccurrence.Word);

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