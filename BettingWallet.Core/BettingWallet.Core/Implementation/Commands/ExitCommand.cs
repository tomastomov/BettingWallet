using BettingWallet.Core.Contracts;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation.Commands
{
    public class ExitCommand : ICommand
    {
        private readonly Action<string> _notify;

        public ExitCommand(Action<string> notify)
        {
            _notify = notify;
        }

        public void Execute()
        {
            _notify?.Invoke(EXIT_MESSAGE);
        }
    }
}
