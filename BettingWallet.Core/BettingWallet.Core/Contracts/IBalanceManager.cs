namespace BettingWallet.Core.Contracts
{
    public interface IBalanceManager
    {
        decimal Balance { get; }

        void Deposit(decimal amount);

        void Withdraw(decimal amount);
    }
}
