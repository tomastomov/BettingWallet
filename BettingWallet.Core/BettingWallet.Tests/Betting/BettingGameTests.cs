using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation;
using BettingWallet.Core.Implementation.Commands;
using Moq;
using NUnit.Framework;
using System;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Tests.Betting
{
    public class BettingGameTests
    {
        private ExitCommand _exitCommand;

        private Mock<ICommandFactory> _mockCommandFactory;
        private Mock<IInputReader> _mockInputReader;
        private Mock<Action<string>> _mockNotify;
        private IGame _bettingGame;

        [SetUp]
        public void Setup()
        {

            _mockCommandFactory = new Mock<ICommandFactory>();
            _mockInputReader = new Mock<IInputReader>();
            _mockNotify = new Mock<Action<string>>();

            _exitCommand = new ExitCommand(_mockNotify.Object);

            _bettingGame = new BettingGame(_mockCommandFactory.Object, _mockInputReader.Object, _mockNotify.Object);
        }

        [Test]
        public void Start_ShouldHandleExitCommandAndExitSuccessfully()
        {
            _mockInputReader.Setup(reader => reader.Read())
                           .Returns(CreateExitCommandInput());

            _mockCommandFactory.SetupSequence(cf => cf.Create(It.IsAny<string[]>()))
                .Returns(_exitCommand);

            _bettingGame.Start();

            VerifyExitCommandExecution();
            VerifySubmitCommandMessage(1);
        }

        [Test]
        public void Start_ShouldHandleDepositCommandAndExitSuccessfully()
        {
            var amount = 100;
            var commandType = DEPOSIT;
            var mockCommand = new Mock<ICommand>();

            _mockInputReader.SetupSequence(reader => reader.Read())
                           .Returns(CreateCommandInput($"{commandType} {amount}"))
                           .Returns(CreateExitCommandInput());

            _mockCommandFactory.SetupSequence(cf => cf.Create(It.IsAny<string[]>()))
                .Returns(mockCommand.Object)
                .Returns(_exitCommand);

            _bettingGame.Start();

            VerifyExitCommandExecution();
            VerifySubmitCommandMessage(2);
        }

        [Test]
        public void Start_ShouldHandleInvalidCommandAndExitSuccessfully()
        {
            var mockCommand = new Mock<ICommand>();
            var commandType = "unsupported";
            var amount = 10;

            _mockInputReader.SetupSequence(reader => reader.Read())
                           .Returns(CreateCommandInput($"{commandType} {amount}"))
                           .Returns(CreateExitCommandInput());

            _mockCommandFactory.SetupSequence(cf => cf.Create(It.IsAny<string[]>()))
                .Returns(mockCommand.Object)
                .Returns(_exitCommand);

            _bettingGame.Start();

            VerifyCommandExecution(mockCommand, GetCommandParametersMatch(commandType, amount.ToString()));
            VerifySubmitCommandMessage(2);
        }

        [Test]
        public void Start_ShoulHandleWithdrawCommandAndExitSuccessfully()
        {
            var amount = 100;
            var commandType = WITHDRAW;
            var mockCommand = new Mock<ICommand>();

            _mockInputReader.SetupSequence(reader => reader.Read())
                           .Returns(CreateCommandInput($"{commandType} {amount}"))
                           .Returns(CreateExitCommandInput());

            _mockCommandFactory.SetupSequence(cf => cf.Create(It.IsAny<string[]>()))
                .Returns(mockCommand.Object)
                .Returns(_exitCommand);

            _bettingGame.Start();

            VerifyCommandExecution(mockCommand, GetCommandParametersMatch(commandType, amount.ToString()));
            VerifySubmitCommandMessage(2);
        }

        [Test]
        public void Start_ShouldHandleBetCommandAndExitSuccessfully()
        {
            var amount = 15;
            var commandType = BET;
            var mockCommand = new Mock<ICommand>();

            _mockInputReader.SetupSequence(reader => reader.Read())
                           .Returns(CreateCommandInput($"{commandType} {amount}"))
                           .Returns(CreateExitCommandInput());

            _mockCommandFactory.SetupSequence(cf => cf.Create(It.IsAny<string[]>()))
                .Returns(mockCommand.Object)
                .Returns(_exitCommand);

            _bettingGame.Start();

            VerifyCommandExecution(mockCommand, GetCommandParametersMatch(commandType, amount.ToString()));
            VerifySubmitCommandMessage(2);
        }

        [Test]
        public void Start_ShouldHandleUnexpectedException()
        {
            var commandType = BET;
            var errorMessage = "test";

            _mockInputReader.SetupSequence(reader => reader.Read())
                           .Returns(CreateCommandInput($"{commandType} 10"))
                           .Returns(CreateExitCommandInput());

            _mockCommandFactory.SetupSequence(cf => cf.Create(It.IsAny<string[]>()))
                .Throws(() => new Exception(errorMessage))
                .Returns(_exitCommand);

            _bettingGame.Start();
            _mockNotify.Verify(n => n($"Something went wrong on our end, please try again! {errorMessage}"), Times.Once);
            VerifySubmitCommandMessage(2);
        }

        private string[] CreateExitCommandInput()
            => CreateCommandInput(EXIT);

        private string[] CreateCommandInput(string command) => command.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        private void VerifyExitCommandExecution()
        {
            _mockCommandFactory.Verify(cf => cf.Create(It.Is<string[]>(p => p[0] == EXIT)), Times.Once);
            _mockNotify.Verify(n => n(EXIT_MESSAGE), Times.Once);
        }

        private void VerifyCommandExecution(Mock<ICommand> mockCommand, Predicate<string[]> match)
        {
            _mockCommandFactory.Verify(cf => cf.Create(It.Is<string[]>(p => match(p))), Times.Once);
            mockCommand.Verify(c => c.Execute(), Times.Once);
        }

        private void VerifySubmitCommandMessage(int count)
        {
            _mockNotify.Verify(n => n(SUBMIT_ACTION_MESSAGE), Times.Exactly(count));
        }

        private Predicate<string[]> GetCommandParametersMatch(string commandType, string amount)
        {
            return p => p[0] == commandType && p[1] == amount;
        }
    }
}
