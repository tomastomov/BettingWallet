using BettingWallet.Core.Contracts;

namespace BettingWallet.Core.Implementation
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random _random;

        public RandomGenerator(Random random)
        {
            _random = random;
        }

        public Outcome GenerateOutcome(int minValue, int maxValue)
        {
            var odd = _random.Next(minValue, maxValue + 1);

            return GetBetPrediction(odd);
        }

        public int GenerateCoefficient(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }

        public decimal GenerateFractionalCoefficient()
        {
            return new decimal(GenerateCoefficient(1, 100)) / 100;
        }

        private Outcome GetBetPrediction(int odd)
                => odd switch
                {
                    <= 50 => Outcome.Loss,
                    <= 90 => Outcome.WinUpToTwoTimes,
                    >= 91 => Outcome.WinUpToTenTimes,
                };
    }
}
