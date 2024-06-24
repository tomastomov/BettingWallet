using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation.Betting;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation.Commands
{
    public class BetCommand : ICommand
    {
        private readonly IBalanceManager _balanceManager;
        private readonly IBettingService _bettingService;
        private readonly Action<string> _notify;
        private readonly decimal _amount;

        public BetCommand(IBalanceManager balanceManager, IBettingService bettingService, Action<string> notify, decimal amount)
        {
            _balanceManager = balanceManager;
            _bettingService = bettingService;
            _notify = notify;
            _amount = amount;
        }

        public void Execute()
        {
            if (_amount < MIN_BETTING_AMOUNT || _amount > MAX_BETTING_AMOUNT)
            {
                _notify?.Invoke(AMOUNT_OUT_OF_RANGE_MESSAGE);
                return;
            }

            _balanceManager.Withdraw(_amount);

            var betResult = _bettingService.Bet(_amount);

            if (betResult.Type == BetResultType.Loss)
            {
                _notify?.Invoke(string.Format(BET_LOSS_MESSAGE, _balanceManager.Balance));
                return;
            }

            _balanceManager.Deposit(betResult.AmountWon ?? 0m);

            _notify.Invoke(string.Format(BET_WIN_MESSAGE, betResult.AmountWon, _balanceManager.Balance));
        }
    }
}
