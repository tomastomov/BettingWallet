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
                _notifier(UNSUPPORTED_COMMAND_MESSAGE);
                return ProcessingResult.CONTINUE;
            }

            var commandType = commandInput[0];

            if (commandType != EXIT && commandInput.Length < 2)
            {
                _notifier(string.Format(INVALID_OPERATION_MESSAGE, commandType));
                return ProcessingResult.CONTINUE;
            }

            var amount = commandInput.Length > 1 ? decimal.Parse(commandInput[1]) : 0;

            ICommand command = null;

            switch (commandType)
            {
                case EXIT:
                    _notifier(EXIT_MESSAGE);
                    return ProcessingResult.EXIT;
                case DEPOSIT:
                    command = new DepositCommand(_balanceManager, _notifier);
                    break;
                case BET:
                    command = new BetCommand(_balanceManager, _bettingService, _notifier);
                    break;
                case WITHDRAW:
                    command = new WithdrawCommand(_balanceManager, _notifier);
                    break;
                default:
                    _notifier(UNSUPPORTED_COMMAND_MESSAGE);
                    return ProcessingResult.CONTINUE;
            }

            command?.Execute(new CommandExecutionContext { Amount = amount });

            return ProcessingResult.CONTINUE;
        }
    }
}
