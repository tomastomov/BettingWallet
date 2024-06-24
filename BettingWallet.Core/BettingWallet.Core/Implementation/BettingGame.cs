using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation.Commands;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Core.Implementation
{
    public class BettingGame : IGame
    {
        private readonly ICommandFactory _commandFactory;
        private readonly IInputReader _inputReader;
        private readonly Action<string> _notifier;

        public BettingGame(ICommandFactory commandFactory, IInputReader inputReader, Action<string> notifier)
        {
            _commandFactory = commandFactory;
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

                    var commandInput = _inputReader.Read();

                    var command = _commandFactory.Create(commandInput);

                    command.Execute();

                    if (command is ExitCommand _)
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
    }
}
