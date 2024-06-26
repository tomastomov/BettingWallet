using BettingWallet.Core.Contracts;
using static BettingWallet.Core.Constants;

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
                throw new InvalidOperationException(INSUFFICIENT_FUNDS_MESSAGE);
            }
        }

        private void EnsurePositiveAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException(AMOUNT_MUST_BE_POSITIVE_MESSAGE);
            }
        }
    }
}
