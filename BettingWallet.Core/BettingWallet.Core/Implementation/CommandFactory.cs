using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation.Commands;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IBalanceManager _balanceManager;
        private readonly IBettingService _bettingService;
        private readonly Action<string> _notify;

        public CommandFactory(IBalanceManager balanceManager, IBettingService bettingService, Action<string> notify)
        {
            _balanceManager = balanceManager;
            _bettingService = bettingService;
            _notify = notify;
        }

        public ICommand Create(string[] parameters)
        {
            var commandType = parameters.Length > 0 ? parameters[0] : string.Empty;
            var amount = 0m;

            if (parameters.Length > 2)
            {
                return CreateInvalidCommand(INVALID_NUMBER_OF_PARAMETERS);
            }

            if (parameters.Length == 2 && !decimal.TryParse(parameters[1], out amount))
            {
                return CreateInvalidCommand(string.Format(INVALID_ARGUMENT_OPERATION_MESSAGE, commandType));
            }

            return GetCommand(commandType, amount);
        }

        private ICommand GetCommand(string commandType, decimal amount)
            => commandType switch
            {
                DEPOSIT => new DepositCommand(_balanceManager, _notify, amount),
                BET => new BetCommand(_balanceManager, _bettingService, _notify, amount),
                WITHDRAW => new WithdrawCommand(_balanceManager, _notify, amount),
                EXIT => new ExitCommand(_notify),
                _ => CreateInvalidCommand(UNSUPPORTED_COMMAND_MESSAGE)
            };

        private ICommand CreateInvalidCommand(string errorMessage)
        {
            return new InvalidCommand(_notify, errorMessage);
        }
    }
}
