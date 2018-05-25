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
        private readonly List<IProfile> profiles;
        private readonly Random random;

        [Required]
        public string Name { get; set; }

        public Bot(string name)
        {
            Name = name;
            profiles = new List<IProfile>();
            random = new Random();
        }

        public void AddProfile(IProfile profile)
        {
            profiles.Add(profile);
        }

        public string GenerateRandomTweetText()
        {
            var tweetText = "";
            WordOccurrence randomWord = null;

            while (true)
            {
                if (randomWord == null)
                    randomWord = GetRandomWordOccurrence(GetRandomProfile());

                else if (randomWord.NextWords.Count > 0)
                    randomWord = GetRandomNextWordOccurrence(randomWord);

                else
                    randomWord = GetRandomWordOccurrence(GetRandomProfile());

                if (tweetText.Length + randomWord.Word.Value.Length > 140)
                    return tweetText;

                tweetText += randomWord.Word.Value + " ";
            }
        }

        private WordOccurrence GetRandomWordOccurrence(IProfile profile)
        {
            var index = random.Next(0, profile.Words.Count);
            var word = profile.Words[index];

            return word;
        }

        private WordOccurrence GetRandomNextWordOccurrence(WordOccurrence word)
        {
            var index = random.Next(0, word.NextWords.Count);
            var retWord = word.NextWords[index].FollowedBy;

            return retWord;
        }

        private IProfile GetRandomProfile()
        {
            return profiles[random.Next(0, profiles.Count)];
        }
    }
}