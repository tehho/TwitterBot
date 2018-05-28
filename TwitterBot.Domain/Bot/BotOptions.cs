using System.Collections.Generic;
using System.Linq;

namespace TwitterBot.Domain
{
    public class BotOptions : Entity
    {
        public string Name { get; set; }
        public List<ProfileOccurrance> Profiles { get; set; }
        public AlgorithmSelector ProfileAlgorithms { get; set; }
        public AlgorithmSelector WordAlgorithms { get; set; }
        //public IReadOnlyList<IProfile> Profiles => Profiles.Select(occ => occ.Profile).ToList();


    public BotOptions()
        {
            Name = "";
            Profiles = new List<ProfileOccurrance>();
            ProfileAlgorithms = new AlgorithmSelector();
            WordAlgorithms = new AlgorithmSelector();
        }

        public void AddProfile(Profile profile)
        {
            if (Profiles.Any(p => p.Profile.Name == profile.Name))
                return;

            Profiles.Add(new ProfileOccurrance(profile, this));
        }
    }
}