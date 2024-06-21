using BettingWallet.Core.Contracts;

namespace BettingWallet.Core.Implementation.Betting
{
    public class BetEarningsCalculator : IEarningsCalculator
    {
        private readonly IOddsGenerator _oddsGenerator;

        public BetEarningsCalculator(IOddsGenerator oddsGenerator)
        {
            _oddsGenerator = oddsGenerator;
        }

        public decimal Calculate(decimal amount, int odd)
        {
            var winAmount = amount + GenerateCoefficient(odd);

            return winAmount;
        }

        private decimal GenerateCoefficient(decimal odd)
            => odd <= Constants.UP_TO_TWO_TIMES_WIN_THRESHOLD ? GenerateCoefficient(1m, 2m) : GenerateCoefficient(2m, 10m); 

        private decimal GenerateCoefficient(decimal min, decimal max)
        {
            var coefficient = _oddsGenerator.GenerateFraction() * (max - min) + min;

            return Math.Round(coefficient, 2);
        }
    }
}
