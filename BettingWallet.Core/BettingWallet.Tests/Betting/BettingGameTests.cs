using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation;
using BettingWallet.Core.Implementation.Betting;
using Moq;
using NUnit.Framework;
using System;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Tests.Betting
{
    public class BettingGameTests
    {
        [Test]
        public void Start_ShouldExitsWhenExitCommandIsReceived()
        {
            var dependencies = GetMockedDependencies();

            dependencies.mockInputReader.Setup(ir => ir.ReadLine()).Returns("exit");

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

            dependencies.mockNotifier.Verify(n => n(EXIT_MESSAGE), Times.Once);
        }

        [Test]
        public void Start_ShouldProcessDepositCommand()
        {
            var amount = 100;
            var dependencies = GetMockedDependencies();

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"deposit {amount}")
                           .Returns("exit");

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

           dependencies.mockBalanceManager.Verify(bm => bm.Add(It.Is<decimal>(d => d == amount)), Times.Once);
        }

        [Test]
        public void Start_ShouldNotifyUnsupportedCommand()
        {
            var dependencies = GetMockedDependencies();

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns("unsupported 10")
                           .Returns("exit");

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

            dependencies.mockNotifier.Verify(n => n(UNSUPPORTED_COMMAND_MESSAGE), Times.Once);
        }

        [Test]
        public void Start_ShouldHandleInvalidAmountArgument()
        {
            var dependencies = GetMockedDependencies();
            var command = "bet";

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"{command} abc")
                           .Returns("exit");

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

            dependencies.mockNotifier.Verify(n => n(string.Format(INVALID_ARGUMENT_OPERATION_MESSAGE, command)), Times.Once);
        }

        [Test]
        public void Start_ShoulHandleWithdrawCommandAndExitSuccessfully()
        {
            var amount = 100;
            var balanceAfterWithdraw = 0;
            var dependencies = GetMockedDependencies();

            dependencies.mockBalanceManager.Setup(bm => bm.Balance).Returns(balanceAfterWithdraw);

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"withdraw {amount}")
                           .Returns("exit");

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

            dependencies.mockBalanceManager.Verify(bm => bm.Subtract(It.Is<decimal>(d => d == amount)), Times.Once);
            dependencies.mockNotifier.Verify(n => n(string.Format(WITHDRAWAL_MESSAGE, amount, balanceAfterWithdraw)), Times.Once);
        }

        [Test]
        public void Start_ShouldHandleBetCommandAndExitSuccessfullyWhenBetIsWon()
        {
            var amount = 10;
            var amountWon = amount + 5;
            var dependencies = GetMockedDependencies();

            dependencies.mockBalanceManager.Setup(bm => bm.Balance).Returns(amountWon);

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"bet {amount}")
                           .Returns("exit");

            dependencies.mockBettingService.Setup(bs => bs.Bet(amount)).Returns(() => new BetResult { AmountWon = amountWon, Type = BetResultType.Win});

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

            dependencies.mockBalanceManager.Verify(bm => bm.Subtract(It.Is<decimal>(d => d == amount)), Times.Once);
            dependencies.mockBalanceManager.Verify(bm => bm.Add(It.Is<decimal>(d => d == amountWon)), Times.Once);
            dependencies.mockNotifier.Verify(n => n(string.Format(BET_WIN_MESSAGE, amountWon, amountWon)), Times.Once);
        }

        [Test]
        public void Start_ShouldHandleBetCommandAndExitSuccessfullyWhenBetIsLost()
        {
            var amount = 10;
            var balance = 0;
            var dependencies = GetMockedDependencies();

            dependencies.mockBalanceManager.Setup(bm => bm.Balance).Returns(balance);

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"bet {amount}")
                           .Returns("exit");

            dependencies.mockBettingService.Setup(bs => bs.Bet(amount)).Returns(() => new BetResult { Type = BetResultType.Loss });

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

            dependencies.mockBalanceManager.Verify(bm => bm.Subtract(It.Is<decimal>(d => d == amount)), Times.Once);
            dependencies.mockNotifier.Verify(n => n(string.Format(BET_LOSS_MESSAGE, balance)), Times.Once);
        }

        [Test]
        public void Start_ShouldHandleBetCommandAndExitSuccessfullyWhenBetAmountIsHigherThanThreshold()
        {
            var amount = 15;
            var dependencies = GetMockedDependencies();

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"bet {amount}")
                           .Returns("exit");

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

            dependencies.mockNotifier.Verify(n => n(AMOUNT_OUT_OF_RANGE_MESSAGE), Times.Once);
        }

        [Test]
        public void Start_ShouldHandleBetCommandAndExitSuccessfullyWhenBetAmountIsLowerThanThreshold()
        {
            var amount = 0.5m;
            var dependencies = GetMockedDependencies();

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"bet {amount}")
                           .Returns("exit");

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

            dependencies.mockNotifier.Verify(n => n(AMOUNT_OUT_OF_RANGE_MESSAGE), Times.Once);
        }

        [Test]
        public void Start_ShouldNotifyInvalidArgumentIfCommandIsNotExitAndThereAreLessThanTwoArguments()
        {
            var dependencies = GetMockedDependencies();
            var operation = "bet";

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"{operation}")
                           .Returns("exit");

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();

            dependencies.mockNotifier.Verify(n => n(string.Format(INVALID_ARGUMENT_OPERATION_MESSAGE, operation)), Times.Once);
        }

        [Test]
        public void Start_ShouldHandleUnexpectedExceptions()
        {
            var dependencies = GetMockedDependencies();
            var operation = "bet";
            var errorMessage = "test";

            dependencies.mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"bet 10")
                           .Returns("exit");

            dependencies.mockBettingService.Setup(b => b.Bet(It.IsAny<decimal>())).Throws(() => new Exception(errorMessage));

            var bettingGame = CreateGame(dependencies);

            bettingGame.Start();
            dependencies.mockNotifier.Verify(n => n($"Something went wrong on our end, please try again! {errorMessage}"), Times.Once);
        }

        private (Mock<IBalanceManager> mockBalanceManager, Mock<IBettingService> mockBettingService, Mock<IInputReader> mockInputReader, Mock<Action<string>> mockNotifier) GetMockedDependencies()
        {
            return (new Mock<IBalanceManager>(), new Mock<IBettingService>(), new Mock<IInputReader>(), new Mock<Action<string>>());
        }

        private IGame CreateGame((Mock<IBalanceManager> mockBalanceManager, Mock<IBettingService> mockBettingService, Mock<IInputReader> mockInputReader, Mock<Action<string>> mockNotifier) dependencies)
        {
            return new BettingGame(dependencies.mockBalanceManager.Object, dependencies.mockBettingService.Object, dependencies.mockInputReader.Object, dependencies.mockNotifier.Object);
        }
    }
}
