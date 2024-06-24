using BettingWallet.Core.Implementation.Betting;

namespace BettingWallet.Core.Contracts
{
    public interface IEarningsCalculator
    {
        decimal Calculate(decimal amount, int lower, int upper);
    }
}
