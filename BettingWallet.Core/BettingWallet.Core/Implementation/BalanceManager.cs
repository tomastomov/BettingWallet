using BettingWallet.Core.Contracts;

namespace BettingWallet.Core.Implementation
{
    public class BalanceManager : IBalanceManager
    {
        private decimal _balance;

        public BalanceManager()
        {
            _balance = 0;
        }

        public decimal Balance => _balance;

        public void Deposit(decimal amount)
        {
            EnsurePositiveAmount(amount);

            _balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            EnsurePositiveAmount(amount);
            EnsureSufficientFunds(amount);

            _balance -= amount;
        }

        private void EnsureSufficientFunds(decimal amount)
        {
            if (amount > _balance)
            {
                throw new InvalidOperationException("Amount must be less than the balance");
            }
        }

        private void EnsurePositiveAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Amount must be a positive number!");
            }
        }
    }
}
