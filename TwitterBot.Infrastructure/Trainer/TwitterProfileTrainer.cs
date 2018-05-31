using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
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
            if (profile == null || content?.Text == null)
                throw new ArgumentNullException();

            var regex = new Regex(@"(\.|,| |!|\?|;|:)");

            var excluded = new Regex(@"(^@|^#|;|:|^http|^HTTP|/|\\|…$|&\S*|^"".*[^""]$|^[^""].*""$|^RT$)");
            //TODO move remove @ and # to tweet generator instead of trainer

            var words = regex.Split(content.Text)
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Where(word => !excluded.IsMatch(word))
                .Select(word => new Word(word)).ToList();

            WordOccurrence lastWordOccurrence = null;
            Word temporaryWord;

            foreach (var word in words)
            {
                if (profile.Vocabulary.Any(w => w.Equals(word)))
                    temporaryWord = profile.Vocabulary.SingleOrDefault(w => w.Equals(word));

                else if (_wordRepository.Get(word) != null)
                    temporaryWord = _wordRepository.Get(word);

                else
                    temporaryWord = _wordRepository.Add(word);

                if (temporaryWord == null)
                    continue;

                var currentWordOccurrence = profile.Words.SingleOrDefault(wo => wo.Word == temporaryWord);

                if (currentWordOccurrence == null)
                    currentWordOccurrence = profile.AddWord(temporaryWord);
                else
                    currentWordOccurrence.Occurrence++;

                lastWordOccurrence?.AddOccurrence(currentWordOccurrence.Word);

                lastWordOccurrence = currentWordOccurrence;
            }

            return profile;
        }
    }
}