using BettingWallet.Core.Contracts;

namespace BettingWallet.Core.Implementation.Commands
{
    public class DepositCommand : ICommand
    {
        private readonly IBalanceManager _balanceManager;
        private readonly Action<string> _notifier;
        public DepositCommand(IBalanceManager balanceManager, Action<string> notifier)
        {
            _balanceManager = balanceManager;
            _notifier = notifier;
        }

        public void Execute(CommandExecutionContext context)
        {
            _balanceManager.Add(context.Amount);
            _notifier?.Invoke(string.Format(Constants.DEPOSIT_MESSAGE, context.Amount, _balanceManager.Balance));
        }
    }
}
