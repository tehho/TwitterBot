using System.Collections.Generic;
using System.Linq;

namespace TwitterBot.Domain
{
    public class BotOption : Entity
    {
        public string Name { get; set; }

        public IReadOnlyList<IProfile> Profiles => ProfileOccurances.Select(occ => occ.Profile).ToList();

        public List<ProfileOccurrance> ProfileOccurances { get; set; }

        public AlgorithmList ProfileAlgorithms;

        public AlgorithmList WordAlgorithms;

        public BotOption()
        {
            Name = "";
            ProfileOccurances = new List<ProfileOccurrance>();
            ProfileAlgorithms = new AlgorithmList();
            WordAlgorithms = new AlgorithmList();
        }

        public void AddProfile(TwitterProfile profile)
        {
            if (Profiles.Any(p => p.Name == profile.Name))
                return;

            ProfileOccurances.Add(new ProfileOccurrance(profile, this));
        }
    }
}