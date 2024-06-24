using BettingWallet.Core.Contracts;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation.Commands
{
    public class WithdrawCommand : ICommand
    {
        private readonly IBalanceManager _balanceManager;
        private readonly Action<string> _notifier;
        private readonly decimal _amount;

        public WithdrawCommand(IBalanceManager balanceManager, Action<string> notifier, decimal amount)
        {
            _balanceManager = balanceManager;
            _notifier = notifier;
            _amount = amount;
        }

        public void Execute()
        {
            _balanceManager.Withdraw(_amount);
            _notifier?.Invoke(string.Format(WITHDRAWAL_MESSAGE, _amount, _balanceManager.Balance));
        }
    }
}
