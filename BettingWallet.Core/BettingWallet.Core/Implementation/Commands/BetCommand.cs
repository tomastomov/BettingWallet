using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation.Betting;

namespace BettingWallet.Core.Implementation.Commands
{
    public class BetCommand : ICommand
    {
        private readonly IBalanceManager _balanceManager;
        private readonly IBettingService _bettingService;
        private readonly Action<string> _notifier;
        public BetCommand(IBalanceManager balanceManager, IBettingService bettingService, Action<string> notifier)
        {
            _balanceManager = balanceManager;
            _bettingService = bettingService;
            _notifier = notifier;
        }
        public void Execute(CommandExecutionContext context)
        {
            var amount = context.Amount;

            if (amount < Constants.MIN_BETTING_AMOUNT || amount > Constants.MAX_BETTING_AMOUNT)
            {
                _notifier?.Invoke(Constants.AMOUNT_OUT_OF_RANGE_MESSAGE);
                return;
            }

            _balanceManager.Subtract(amount);

            var betResult = _bettingService.Bet(amount);

            if (betResult.Type == BetResultType.Loss)
            {
                _notifier?.Invoke(string.Format(Constants.BET_LOSS_MESSAGE, _balanceManager.Balance));
                return;
            }

            _balanceManager.Add(betResult.AmountWon ?? 0m);

            _notifier.Invoke(string.Format(Constants.BET_WIN_MESSAGE, betResult.AmountWon, _balanceManager.Balance));
        }
    }
}
