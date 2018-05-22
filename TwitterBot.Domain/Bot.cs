﻿using System;
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
            Word randomWord = null;

            while (true)
            {
                if (randomWord == null)
                    randomWord = GetRandomWord(GetRandomProfile());

                else if (randomWord.NextWord != null)
                    randomWord = GetRandomNextWord(randomWord);

                else
                    randomWord = GetRandomWord(GetRandomProfile());

                if (tweetText.Length + randomWord.Value.Length > 140)
                    return tweetText;

                tweetText += randomWord.Value + " ";
            }
        }

        private Word GetRandomWord(IProfile profile)
        {
            return profile.Words[random.Next(0, profile.Words.Count)].Word;
        }

        private Word GetRandomNextWord(Word word)
        {
            return word.NextWord?[random.Next(0, word.NextWord.Count)].Word;
        }

        private IProfile GetRandomProfile()
        {
            return profiles[random.Next(0, profiles.Count)];
        }
    }
}