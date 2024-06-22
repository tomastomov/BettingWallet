using BettingWallet.Core.Contracts;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation.Commands
{
    public class WithdrawCommand : ICommand
    {
        private readonly IBalanceManager _balanceManager;
        private readonly Action<string> _notifier;
        public WithdrawCommand(IBalanceManager balanceManager, Action<string> notifier)
        {
            _balanceManager = balanceManager;
            _notifier = notifier;
        }

        public void Execute(CommandExecutionContext context)
        {
            _balanceManager.Subtract(context.Amount);
            _notifier?.Invoke(string.Format(WITHDRAWAL_MESSAGE, context.Amount, _balanceManager.Balance));
        }
    }
}
