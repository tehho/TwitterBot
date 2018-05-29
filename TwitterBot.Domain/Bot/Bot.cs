using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterBot.Domain
{
    public class Bot : Entity
    {
        private readonly BotOptions options;
        private readonly Random random;

        public string Name { get; set; }

        public Bot(BotOptions options)
        {
            this.options = options;
            random = new Random();
        }
        
        public Tweet GenerateTweet()
        {
            var tweetText = "";
            Word previousWord = null;

            while (true)
            {
                var profile = PickProfile(options);

                var word = PickWord(profile, previousWord, options);

                if (tweetText.Length + word.Value.Length > 140)
                    return new Tweet(tweetText);

                tweetText += word.Value + " ";

                previousWord = word;
            }
        }

        private Word PickWord(TwitterProfile profile, Word previousWord, BotOptions options)
        {
            var algorithm = options.WordAlgorithms.PickAlgorithm(random);
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
                    word = PickWordByProbabilityWithPrediction(profile, previousWord);
                    break;
            }

            return word;
        }

        private Word PickWordByProbabilityWithPrediction(TwitterProfile profile, Word previousWord)
        {
            var word = profile.Words.SingleOrDefault(w => w.Word.Equals(previousWord));

            if (word == null)
                return PickWordByProbability(profile);

            var weights = word.NextWordOccurrences.Select(n => n.Occurrence).ToList();
            var index = AlgorithmSelector.PickIndexWeighted(weights, random);

            return word.NextWordOccurrences[index].Word;

        }

        private Word PickWordByProbability(TwitterProfile profile)
        {
            var weights = profile.Words.Select(w => w.Occurrence).ToList();

            var index = AlgorithmSelector.PickIndexWeighted(weights, random);

            return profile.Words[index].Word;
        }

        private Word PickWordRandom(TwitterProfile profile)
        {
            var index = random.Next(profile.Words.Count);

            return profile.Words[index].Word;
        }

        private TwitterProfile PickProfile(BotOptions options)
        {
            var algorithm = options.ProfileAlgorithms.PickAlgorithm(random);
            TwitterProfile profile = null;

            switch (algorithm)
            {
                case AlgorithmType.Random:
                    profile = PickProfileTrueRandom(options.Profiles);
                    break;
                case AlgorithmType.ByProbability:
                    profile = PickProfileWeighted(options.ProfileOccurances);
                    break;
                default:
                    profile = PickProfileTrueRandom(options.Profiles);
                    break;
            }

            return profile;
        }

        private TwitterProfile PickProfileWeighted(IReadOnlyList<ProfileOccurrance> optionsProfileOccurances)
        {
            var weights = optionsProfileOccurances.Select(occ => occ.Occurrance).ToList();

            var index = AlgorithmSelector.PickIndexWeighted(weights, random);

            var profile = optionsProfileOccurances[index].Profile;

            return profile;
        }

        private TwitterProfile PickProfileTrueRandom(IReadOnlyList<TwitterProfile> profiles)
        {
            var index = random.Next(profiles.Count);
            var profile = profiles[index];

            return profile;
        }

    }
}