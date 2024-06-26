using BettingWallet.Core.Contracts;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation.Betting
{
    public class BettingService : IBettingService
    {
        private const int MIN_ODD = 1;
        private const int MAX_ODD = 100;

        private readonly IRandomGenerator _randomGenerator;
        private readonly IEarningsCalculator _earningsCalculator;

        public BettingService(IRandomGenerator randomGenerator, IEarningsCalculator earningsCalculator)
        {
            _randomGenerator = randomGenerator;
            _earningsCalculator = earningsCalculator;
        }

        public BetResult Bet(decimal amount)
        {
            var betOutcome = _randomGenerator.GenerateOutcome(MIN_ODD, MAX_ODD);

            if (betOutcome == Outcome.Loss)
            {
                return new BetResult { Type = BetResultType.Loss };
            }

            var coefficients = GetCofficientRange(betOutcome);

            var amountEarned = _earningsCalculator.Calculate(amount, coefficients.coefficientLower, coefficients.coefficientUpper);

            return new BetResult { Type = BetResultType.Win, AmountWon = amountEarned };
        }

        private (int coefficientLower, int coefficientUpper) GetCofficientRange(Outcome prediction)
            => prediction switch
            {
                Outcome.Loss => (0, 0),
                Outcome.WinUpToTwoTimes => (UP_TO_TWO_TIMES_COEFFICIENT_LOWER_BOUND, UP_TO_TWO_TIMES_COEFFICIENT_UPPER_BOUND),
                Outcome.WinUpToTenTimes => (UP_TO_TEN_TIMES_COEFFICIENT_LOWER_BOUND, UP_TO_TEN_TIMES_COEFFICIENT_UPPER_BOUND),
                _ => throw new ArgumentException(INVALID_OUTCOME_MESSAGE),
            };
    }
}
