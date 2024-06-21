namespace BettingWallet.Core.Contracts
{
    public interface IEarningsCalculator
    {
        decimal Calculate(decimal amount, int odd);
    }
}
