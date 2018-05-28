using System.Collections.Generic;
using System.Linq;

namespace TwitterBot.Domain
{
    public class BotOptions : Entity
    {
        public string Name { get; set; }
        public IReadOnlyList<IProfile> Profiles => ProfileOccurances.Select(occ => occ.Profile).ToList();
        public List<ProfileOccurrance> ProfileOccurances { get; set; }
        public AlgorithmSelector ProfileAlgorithms;
        public AlgorithmSelector WordAlgorithms;

        public BotOptions()
        {
            Name = "";
            ProfileOccurances = new List<ProfileOccurrance>();
            ProfileAlgorithms = new AlgorithmSelector();
            WordAlgorithms = new AlgorithmSelector();
        }

        public void AddProfile(IProfile profile)
        {
            if (Profiles.Any(p => p.Name == profile.Name))
                return;

            ProfileOccurances.Add(new ProfileOccurrance(profile, this));
        }
    }
}