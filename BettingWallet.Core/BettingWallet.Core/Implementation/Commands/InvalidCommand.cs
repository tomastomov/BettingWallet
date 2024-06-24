using BettingWallet.Core.Contracts;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation.Commands
{
    public class InvalidCommand : ICommand
    {
        private readonly Action<string> _notify;
        private readonly string _errorMessage;

        public InvalidCommand(Action<string> notify, string errorMessage)
        {
            _notify = notify;
            _errorMessage = errorMessage;
        }

        public void Execute()
        {
            _notify?.Invoke(_errorMessage);
        }
    }
}
