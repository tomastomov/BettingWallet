using BettingWallet.Core.Contracts;

namespace BettingWallet.Core.Implementation.Betting
{
    public class BetEarningsCalculator : IEarningsCalculator
    {
        private readonly IRandomGenerator _randomGenerator;

        public BetEarningsCalculator(IRandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        public decimal Calculate(decimal amount, int lower, int upper)
        {
            decimal coefficient = _randomGenerator.GenerateCoefficient(lower, upper);

            if (coefficient != upper)
            {
                coefficient += _randomGenerator.GenerateFractionalCoefficient();
            }

            return amount * coefficient;
        }
    }
}
