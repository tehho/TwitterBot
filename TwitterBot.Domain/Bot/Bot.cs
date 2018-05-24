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
        private readonly BotOption _options;
        private readonly Random random;

        public Bot(BotOption options)
        {
            _options = options;
            random = new Random();
        }
        
        public string Name { get; set; }

        public Tweet GenerateRandomTweetText()
        {
            var tweetText = "";
            Word prevWord = null;

            while (true)
            {
                var profile = PickProfile(_options);

                var nextWord = PickWord(profile, prevWord, _options);

                if (tweetText.Length + nextWord.Value.Length > 140)
                    return new Tweet(tweetText);

                tweetText += nextWord.Value + " ";

                prevWord = nextWord;

            }
        }

        private Word PickWord(IProfile profile, Word prevWord, BotOption options)
        {
            var algo = options.WordAlgorithms.PickAlgorithm(random);
            Word word = null;
            switch (algo)
            {
                case AlgorithmType.TrueRandom:
                    word = PickWordTrueRandom(profile);
                    break;
                case AlgorithmType.WeightedRandom:

                    word = PickWordWeightedRandom(profile);

                    break;
                case AlgorithmType.WeightedPrediction:
                    word = PickWordWeightedPrediction(profile, prevWord);

                    break;
            }

            return word;
        }

        private Word PickWordWeightedPrediction(IProfile profile, Word prevWord)
        {
            var pword = profile.WordList.SingleOrDefault(wco => wco.WordContainer.Word.Value == prevWord.Value);

            if (pword == null)
                return PickWordWeightedRandom(profile);

            var weights = pword.WordContainer.Occurrances.Select(occ => occ.Occurrance).ToList();
            var index = AlgorithmList.PickIndexWeighted(weights, random);

            var word = pword.WordContainer.NextWords[index];

            return word;

        }

        private Word PickWordWeightedRandom(IProfile profile)
        {
            var weights = profile.WordList.Select(occ => occ.Occurrence).ToList();
            var index = AlgorithmList.PickIndexWeighted(weights, random);

            var word = profile.Vocabulary[index];

            return word;
        }

        private Word PickWordTrueRandom(IProfile profile)
        {
            var index = random.Next(profile.Vocabulary.Count);
            var word = profile.Vocabulary[index];

            return word;
        }

        private IProfile PickProfile(BotOption options)
        {
            var rand = options.ProfileAlgorithms.PickAlgorithm(random);

            IProfile profile = null;

            switch (rand)
            {
                case AlgorithmType.TrueRandom:
                    profile = PickProfileTrueRandom(options.Profiles);
                    break;
                case AlgorithmType.WeightedRandom:
                    profile = PickProfileWeighted(options.ProfileOccurances);
                    break;
                default:
                    profile = PickProfileTrueRandom(options.Profiles);
                
                    break;
            }

            return profile;
        }

        private IProfile PickProfileWeighted(IReadOnlyList<ProfileOccurrance> optionsProfileOccurances)
        {
            var weights = optionsProfileOccurances.Select(occ => occ.Occurrance).ToList();

            var index = AlgorithmList.PickIndexWeighted(weights, random);

            var profile = optionsProfileOccurances[index].Profile;

            return profile;
        }

        private IProfile PickProfileTrueRandom(IReadOnlyList<IProfile> profiles)
        {
            var index = random.Next(profiles.Count);
            var profile = profiles[index];

            return profile;
        }

    }
}