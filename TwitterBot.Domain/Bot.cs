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
        private readonly Dictionary<IProfile, int> profiles;
        private readonly Random random;

        [Required]
        public string Name { get; set; }
        
        public Bot(string name)
        {
            Name = name;
            profiles = new Dictionary<IProfile, int>();
            random = new Random();
        }

        public void AddProfile(IProfile profile)
        {
            profiles.Add(profile, 1);
        }

        public string GenerateRandomTweetText()
        {
            var tweetText = "";

            while (true)
            {
                var nextRandomWord = GetRandomWord();

                if (tweetText.Length + nextRandomWord.Length > 140)
                    return tweetText;

                tweetText += nextRandomWord;
            }
        }

        private string GetRandomWord()
        {
            var profile = GetRandomProfile();

            return profile.Words.ElementAt(random.Next(0, profile.Words.Count)).Key.Value;
        }

        private IProfile GetRandomProfile()
        {
            return profiles.ElementAt(random.Next(0, profiles.Count)).Key;
        }
    }
}