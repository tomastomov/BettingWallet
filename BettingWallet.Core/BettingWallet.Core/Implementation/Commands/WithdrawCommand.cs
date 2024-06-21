using BettingWallet.Core.Contracts;

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
            _notifier?.Invoke(string.Format(Constants.WITHDRAWAL_MESSAGE, context.Amount, _balanceManager.Balance));
        }
    }
}
