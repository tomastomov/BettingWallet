using BettingWallet.Core.Implementation.Betting;

namespace BettingWallet.Core.Contracts
{
    public interface IBettingService
    {
        BetResult Bet(decimal amount);
    }
}
