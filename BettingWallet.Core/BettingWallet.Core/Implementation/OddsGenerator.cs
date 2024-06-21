using BettingWallet.Core.Contracts;

namespace BettingWallet.Core.Implementation
{
    public class OddsGenerator : IOddsGenerator
    {
        private readonly Random _random;

        public OddsGenerator(Random random)
        {
            _random = random;
        }

        public int Generate(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }

        public decimal GenerateFraction()
        {
            return new decimal(_random.NextDouble());
        }
    }
}
