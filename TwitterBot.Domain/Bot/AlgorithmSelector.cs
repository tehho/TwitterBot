using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterBot.Domain
{
    public class AlgorithmSelector : Entity
    {
        //public Guid BotOptionsId { get; set; }
        //public BotOptions BotOptions { get; set; }

        public int Random { get; set; }
        public int ByProbability { get; set; }
        public int ByProbabilityWithPrediction { get; set; }
        public int Sum => Random + ByProbabilityWithPrediction + ByProbability;

        public AlgorithmSelector()
        {
            Random = 0;
            ByProbability = 0;
            ByProbabilityWithPrediction = 0;
        }

        public AlgorithmType PickAlgorithm(Random random)
        {
            if (Sum <= 0)
                return AlgorithmType.Random;

            var index = PickIndexWeighted(new List<int> {
                Random,
                ByProbability,
                ByProbabilityWithPrediction
            }, random);

            switch (index)
            {
                case 0:
                    return AlgorithmType.Random;
                case 1:
                    return AlgorithmType.ByProbability;
                case 2:
                    return AlgorithmType.ByProbabilityWithPrediction;
                default:
                    return AlgorithmType.Random;
            }
        }

        public static int PickIndexWeighted(IReadOnlyList<int> weights, Random random)
        {
            var rnd = random.Next(weights.Sum());

            for (var i = 0; i < weights.Count; i++)
            {
                if (rnd < weights[i])
                    return i;

                rnd -= weights[i];
            }

            return 0;
        }
    }
}