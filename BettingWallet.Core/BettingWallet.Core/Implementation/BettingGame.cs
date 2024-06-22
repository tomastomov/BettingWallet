using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation.Commands;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation
{
    public class BettingGame : IGame
    {
        private readonly IBalanceManager _balanceManager;
        private readonly IBettingService _bettingService;
        private readonly IInputReader _inputReader;
        private readonly Action<string> _notifier;

        public BettingGame(IBalanceManager balanceManager, IBettingService bettingService, IInputReader inputReader, Action<string> notifier)
        {
            _balanceManager = balanceManager;
            _bettingService = bettingService;
            _inputReader = inputReader;
            _notifier = notifier;
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    _notifier(SUBMIT_ACTION_MESSAGE);

                    var commandInput = _inputReader.ReadLine()?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

                    var processingResult = ProcessCommand(commandInput);

                    if (processingResult == ProcessingResult.EXIT)
                    {
                        return;
                    }
                } 
                catch (InvalidOperationException e)
                {
                    _notifier(e.Message);
                } 
                catch (Exception e)
                {
                    _notifier($"Something went wrong on our end, please try again! {e.Message}");
                }
                
            }
        }
        private ProcessingResult ProcessCommand(string[] commandInput)
        {
            if (commandInput == null || commandInput.Length == 0)
            {
                return NotifyUnSupportedCommand();
            }

            var commandType = commandInput[0];

            if (commandType == EXIT)
            {
                _notifier(EXIT_MESSAGE);
                return ProcessingResult.EXIT;
            }

            if (commandInput.Length < 2)
            {
                return NotifyInvalidArgument(commandType);
            }

            if (!decimal.TryParse(commandInput[1], out var amount))
            {
                return NotifyInvalidArgument(commandType);
            }

            ICommand command = GetComamnd(commandType);

            if (command == null)
            {
                return NotifyUnSupportedCommand();
            }

            command.Execute(new CommandExecutionContext { Amount = amount });

            return ProcessingResult.CONTINUE;
        }

        private ICommand GetComamnd(string commandType) => commandType switch
        {
            DEPOSIT => new DepositCommand(_balanceManager, _notifier),
            BET => new BetCommand(_balanceManager, _bettingService, _notifier),
            WITHDRAW => new WithdrawCommand(_balanceManager, _notifier),
            _ => null,
        };

        private ProcessingResult NotifyUnSupportedCommand()
        {
            _notifier(UNSUPPORTED_COMMAND_MESSAGE);
            return ProcessingResult.CONTINUE;
        }

        private ProcessingResult NotifyInvalidArgument(string commandType)
        {
            _notifier(string.Format(INVALID_ARGUMENT_OPERATION_MESSAGE, commandType));
            return ProcessingResult.CONTINUE;
        }
    }
}
