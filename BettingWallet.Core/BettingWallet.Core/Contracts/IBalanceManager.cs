namespace BettingWallet.Core.Contracts
{
    public interface IBalanceManager
    {
        decimal Balance { get; }

        void Add(decimal amount);

        void Subtract(decimal amount);
    }
}
