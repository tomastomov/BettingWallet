using BettingWallet.Core.Contracts;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation.Commands
{
    public class DepositCommand : ICommand
    {
        private readonly IBalanceManager _balanceManager;
        private readonly Action<string> _notify;
        private readonly decimal _amount;

        public DepositCommand(IBalanceManager balanceManager, Action<string> notify, decimal amount)
        {
            _balanceManager = balanceManager;
            _notify = notify;
            _amount = amount;
        }

        public void Execute()
        {
            _balanceManager.Deposit(_amount);
            _notify?.Invoke(string.Format(DEPOSIT_MESSAGE, _amount, _balanceManager.Balance));
        }
    }
}
