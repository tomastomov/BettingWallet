using BettingWallet.Core.Contracts;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation.Betting
{
    public class BettingService : IBettingService
    {
        private readonly IOddsGenerator _oddsGenerator;
        private readonly IEarningsCalculator _earningsCalculator;

        public BettingService(IOddsGenerator oddsGenerator, IEarningsCalculator earningsCalculator)
        {
            _oddsGenerator = oddsGenerator;
            _earningsCalculator = earningsCalculator;
        }

        public BetResult Bet(decimal amount)
        {
            var odd = _oddsGenerator.Generate(1, 100);

            if (IsBetLost(odd))
            {
                return new BetResult { Type = BetResultType.Loss };
            }

            var amountEarned = _earningsCalculator.Calculate(amount, odd);

            return new BetResult { Type = BetResultType.Win, AmountWon = amountEarned };
        }

        private bool IsBetLost(int odd)
        {
            return odd <= WIN_THRESHOLD;
        }
    }
}
