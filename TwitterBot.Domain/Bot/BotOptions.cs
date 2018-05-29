using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TwitterBot.Domain
{
    public class BotOptions : Entity
    {
        public string Name { get; set; }
        [NotMapped]
        public IReadOnlyList<TwitterProfile> Profiles => ProfileOccurances.Select(occ => occ.Profile).ToList();
        public List<ProfileOccurrance> ProfileOccurances { get; set; }
        public Guid ProfileAlgorithmsId { get; set; }
        public ProfileAlgorithmSelector ProfileAlgorithms { get; set; }
        public Guid WordAlgorithmsId { get; set; }
        public AlgorithmSelector WordAlgorithms { get; set; }

        public BotOptions()
        {
            Name = "";
            ProfileOccurances = new List<ProfileOccurrance>();
            ProfileAlgorithms = new ProfileAlgorithmSelector();
            WordAlgorithms = new AlgorithmSelector();
        }

        public void AddProfile(TwitterProfile profile)
        {
            if (Profiles.Any(p => p.Name == profile.Name))
                return;

            ProfileOccurances.Add(new ProfileOccurrance(profile, this));
        }
    }
}