﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBot.Domain
{
    public class ProfileOccurrance
    {
        public Guid ProfileId { get; set; }
        [Required]
        public TwitterProfile Profile { get; set; }
        public Guid BotOptionsId { get; set; }
        [Required]
        public BotOptions BotOptions { get; set; }
        public int Occurrence { get; set; }
        
        public ProfileOccurrance()
        {
        }

        public ProfileOccurrance(TwitterProfile profile, BotOptions options, int occurrence)
        {
            Profile = profile;
            ProfileId = profile.Id.Value;

            BotOptions = options;
            BotOptionsId = options.Id.Value;

            Occurrence = occurrence;
        }

        public ProfileOccurrance(TwitterProfile profile, BotOptions options)
        {
            Profile = profile;
            ProfileId = profile.Id.Value;

            BotOptions = options;
            BotOptionsId = options.Id.Value;

            Occurrence = 1;
        }
    }
}