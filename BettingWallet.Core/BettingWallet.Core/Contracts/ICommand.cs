using BettingWallet.Core.Implementation.Commands;

namespace BettingWallet.Core.Contracts
{
    public interface ICommand
    {
        void Execute(CommandExecutionContext context);
    }
}
