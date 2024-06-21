namespace BettingWallet.Core.Contracts
{
    public interface IOddsGenerator
    {
        int Generate(int minValue, int maxValue);

        decimal GenerateFraction();
    }
}
