using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterBot.Domain;

namespace TwitterBot.Api.Model
{
    public class BotOptionApi
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public List<TwitterProfileApi> Profiles { get; set; }

        public AlgorithmType? WordAlgorithm { get; set; }
        public AlgorithmType? ProfileAlgorithm { get; set; }

        public BotOptionApi()
        {
            Id = null;
            Name = "";
            Profiles = new List<TwitterProfileApi>();
        }

        public static implicit operator BotOptions(BotOptionApi value)
        {
            ProfileAlgorithmSelector profileAlg = null;

            if (value.ProfileAlgorithm == null)
                profileAlg = new ProfileAlgorithmSelector()
                {
                    Random =  1
                };
            else
            {
                switch (value.ProfileAlgorithm.Value)
                {
                    case AlgorithmType.Random:
                        profileAlg = new ProfileAlgorithmSelector()
                        {
                            Random =  1
                        };
                        break;
                    case AlgorithmType.ByProbability:
                        profileAlg = new ProfileAlgorithmSelector()
                        {
                            ByProbability = 1
                        };
                        break;
                    default:
                        profileAlg = new ProfileAlgorithmSelector()
                        {
                            Random = 1
                        };
                        break;
                }
            }
            AlgorithmSelector WordAlg = null;

            if (value.WordAlgorithm == null)
                WordAlg = new AlgorithmSelector()
                {
                    Random =  1
                };
            else
            {
                switch (value.WordAlgorithm.Value)
                {
                    case AlgorithmType.Random:
                        WordAlg = new AlgorithmSelector()
                        {
                            Random = 1
                        };
                        break;
                    case AlgorithmType.ByProbability:
                        WordAlg = new AlgorithmSelector()
                        {
                            ByProbability = 1
                        };
                        break;
                    case AlgorithmType.ByProbabilityWithPrediction:
                        WordAlg = new AlgorithmSelector()
                        {
                            ByProbabilityWithPrediction = 1
                        };
                        break;
                    default:
                        WordAlg = new AlgorithmSelector()
                        {
                            Random = 1
                        };
                        break;
                }
            }
            

            return new BotOptions
            {
                Name = value.Name,
                ProfileAlgorithms = profileAlg,
                WordAlgorithms = WordAlg
            };
        }
    }
}
