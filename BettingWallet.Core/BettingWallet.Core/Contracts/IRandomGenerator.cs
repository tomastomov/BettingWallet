using BettingWallet.Core.Implementation;

namespace BettingWallet.Core.Contracts
{
    public interface IRandomGenerator
    {
        Outcome GenerateOutcome(int minValue, int maxValue);

        int GenerateCoefficient(int minValue, int maxValue);

        decimal GenerateFractionalCoefficient();
    }
}
