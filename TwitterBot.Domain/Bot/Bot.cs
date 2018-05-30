using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterBot.Domain
{
    public class Bot : Entity
    {
        private readonly BotOptions _options;
        private readonly Random _random;

        public string Name { get; set; }

        public Bot(BotOptions options)
        {
            _options = options;
            _random = new Random();
        }
        
        public Tweet GenerateTweet()
        {
            var tweetText = "";
            Word previousWord = null;
            var lastWordWasStop = true;

            while (true)
            {
                var profile = PickProfile();

                var word = PickWord(profile, previousWord);

                if (tweetText.Length + word.Value.Length > 140)
                    return new Tweet(tweetText.TrimEnd());

                if (IsStop(word.Value) && lastWordWasStop)
                {
                    previousWord = null;
                    continue;
                }

                if (IsSeparator(word.Value) || IsStop(word.Value))
                {
                    tweetText = tweetText.TrimEnd();
                    lastWordWasStop = IsStop(word.Value);
                }

                tweetText += lastWordWasStop ? FirstCharacterToUppercase(word.Value) : word.Value;

                lastWordWasStop = IsStop(word.Value);

                tweetText += " ";

                previousWord = word;
            }
        }

        private Word PickWord(TwitterProfile profile, Word previousWord)
        {
            var algorithm = _options.WordAlgorithms.PickAlgorithm(_random);
            Word word = null;

            switch (algorithm)
            {
                case AlgorithmType.Random:
                    word = PickWordRandom(profile);
                    break;
                case AlgorithmType.ByProbability:
                    word = PickWordByProbability(profile);
                    break;
                case AlgorithmType.ByProbabilityWithPrediction:
                    word = previousWord == null ?
                        PickWordByProbability(profile) :
                        PickWordByProbabilityWithPrediction(profile, previousWord);
                    break;
            }

            return word;
        }

        private Word PickWordByProbabilityWithPrediction(TwitterProfile profile, Word previousWord)
        {
            var word = profile.Words.SingleOrDefault(w => w.Word.Equals(previousWord));
            
            if (word?.NextWordOccurrences == null || word.NextWordOccurrences.Count == 0)
                return PickWordByProbability(profile);

            var weights = word.NextWordOccurrences.Select(n => n.Occurrence).ToList();
            var index = AlgorithmSelector.PickIndexWeighted(weights, _random);

            return word.NextWordOccurrences[index].Word;
        }

        private Word PickWordByProbability(TwitterProfile profile)
        {
            var weights = profile.Words.Select(w => w.Occurrence).ToList();

            var index = AlgorithmSelector.PickIndexWeighted(weights, _random);

            return profile.Words[index].Word;
        }

        private Word PickWordRandom(TwitterProfile profile)
        {
            var index = _random.Next(profile.Words.Count);

            return profile.Words[index].Word;
        }

        private TwitterProfile PickProfile()
        {
            var algorithm = _options.ProfileAlgorithms.PickAlgorithm(_random);
            TwitterProfile profile;

            switch (algorithm)
            {
                case AlgorithmType.Random:
                    profile = PickProfileRandom(_options.Profiles);
                    break;
                case AlgorithmType.ByProbability:
                    profile = PickProfileWeighted(_options.ProfileOccurances);
                    break;
                default:
                    profile = PickProfileRandom(_options.Profiles);
                    break;
            }

            return profile;
        }

        private TwitterProfile PickProfileWeighted(IReadOnlyList<ProfileOccurrance> optionsProfileOccurances)
        {
            var weights = optionsProfileOccurances.Select(occ => occ.Occurrence).ToList();

            var index = AlgorithmSelector.PickIndexWeighted(weights, _random);

            var profile = optionsProfileOccurances[index].Profile;

            return profile;
        }

        private TwitterProfile PickProfileRandom(IReadOnlyList<TwitterProfile> profiles)
        {
            var index = _random.Next(profiles.Count);
            var profile = profiles[index];

            return profile;
        }

        private bool IsStop(string word)
        {
            return Regex.IsMatch(word.Last().ToString(), @"(\.|!|\?)");
        }

        private bool IsSeparator(string word)
        {
            return Regex.IsMatch(word.Last().ToString(), @"(,|;|:)");
        }

        private string FirstCharacterToUppercase(string word)
        {
            return word[0].ToString().ToUpper() + word.Substring(1);
        }
    }
}