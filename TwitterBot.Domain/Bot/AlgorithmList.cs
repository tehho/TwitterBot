using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterBot.Domain
{
    public class AlgorithmList
    {
        public int BotOptionsId { get; set; }
        public BotOption botOption { get; set; }

        public int TrueRandom { get; set; }
        public int WeightedRandom { get; set; }
        public int WeightedPrediction { get; set; }

        public int Sum => TrueRandom + WeightedPrediction + WeightedRandom;

        public AlgorithmList()
        {
            TrueRandom = 0;
            WeightedRandom = 0;
            WeightedPrediction = 0;
        }

        public AlgorithmList Empyt()
        {
            return new AlgorithmList();
        }

        public AlgorithmType PickAlgorithm(Random random)
        {
            if (Sum <= 0)
                return AlgorithmType.TrueRandom;

            var index = PickIndexWeighted(new List<int> {
                TrueRandom,
                WeightedRandom,
                WeightedPrediction
            }, random);

            if (index == 0)
                return AlgorithmType.TrueRandom;
            if (index == 1)
                return AlgorithmType.WeightedRandom;
            if (index == 2)
                return AlgorithmType.WeightedPrediction;

            return AlgorithmType.TrueRandom;
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

    public enum AlgorithmType
    {
        TrueRandom = 1,
        WeightedRandom,
        WeightedPrediction
    }

}