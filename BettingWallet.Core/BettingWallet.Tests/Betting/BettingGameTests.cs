//using BettingWallet.Core.Contracts;
//using BettingWallet.Core.Implementation;
//using BettingWallet.Core.Implementation.Betting;
//using Moq;
//using NUnit.Framework;
//using System;
//using static BettingWallet.Core.Constants;

//namespace BettingWallet.Tests.Betting
//{
//    public class BettingGameTests
//    {
//        [Test]
//        public void Start_ShouldExitsWhenExitCommandIsReceived()
//        {
//            var dependencies = GetMockedDependencies();

//            dependencies.mockInputReader.Setup(ir => ir.Read()).Returns(new string[] { "exit"});

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockNotifier.Verify(n => n(EXIT_MESSAGE), Times.Once);
//        }

//        [Test]
//        public void Start_ShouldProcessDepositCommand()
//        {
//            var amount = 100;
//            var dependencies = GetMockedDependencies();

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput($"deposit {amount}"))
//                           .Returns(CreateExitCommand());

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockBalanceManager.Verify(bm => bm.Deposit(It.Is<decimal>(d => d == amount)), Times.Once);
//        }

//        [Test]
//        public void Start_ShouldNotifyUnsupportedCommand()
//        {
//            var dependencies = GetMockedDependencies();

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput("unsupported 10"))
//                           .Returns(CreateExitCommand());

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockNotifier.Verify(n => n(UNSUPPORTED_COMMAND_MESSAGE), Times.Once);
//        }

//        [Test]
//        public void Start_ShouldHandleInvalidAmountArgument()
//        {
//            var dependencies = GetMockedDependencies();
//            var command = "bet";

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput($"{command} abc"))
//                           .Returns(CreateExitCommand());

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockNotifier.Verify(n => n(string.Format(INVALID_ARGUMENT_OPERATION_MESSAGE, command)), Times.Once);
//        }

//        [Test]
//        public void Start_ShoulHandleWithdrawCommandAndExitSuccessfully()
//        {
//            var amount = 100;
//            var balanceAfterWithdraw = 0;
//            var dependencies = GetMockedDependencies();

//            dependencies.mockBalanceManager.Setup(bm => bm.Balance).Returns(balanceAfterWithdraw);

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput($"withdraw {amount}"))
//                           .Returns(CreateExitCommand());

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockBalanceManager.Verify(bm => bm.Withdraw(It.Is<decimal>(d => d == amount)), Times.Once);
//            dependencies.mockNotifier.Verify(n => n(string.Format(WITHDRAWAL_MESSAGE, amount, balanceAfterWithdraw)), Times.Once);
//        }

//        [Test]
//        public void Start_ShouldHandleBetCommandAndExitSuccessfullyWhenBetIsWon()
//        {
//            var amount = 10;
//            var amountWon = amount + 5;
//            var dependencies = GetMockedDependencies();

//            dependencies.mockBalanceManager.Setup(bm => bm.Balance).Returns(amountWon);

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput($"bet {amount}"))
//                           .Returns(CreateExitCommand());

//            dependencies.mockBettingService.Setup(bs => bs.Bet(amount)).Returns(() => new BetResult { AmountWon = amountWon, Type = BetResultType.Win });

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockBalanceManager.Verify(bm => bm.Withdraw(It.Is<decimal>(d => d == amount)), Times.Once);
//            dependencies.mockBalanceManager.Verify(bm => bm.Deposit(It.Is<decimal>(d => d == amountWon)), Times.Once);
//            dependencies.mockNotifier.Verify(n => n(string.Format(BET_WIN_MESSAGE, amountWon, amountWon)), Times.Once);
//        }

//        [Test]
//        public void Start_ShouldHandleBetCommandAndExitSuccessfullyWhenBetIsLost()
//        {
//            var amount = 10;
//            var balance = 0;
//            var dependencies = GetMockedDependencies();

//            dependencies.mockBalanceManager.Setup(bm => bm.Balance).Returns(balance);

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput($"bet {amount}"))
//                           .Returns(CreateExitCommand());

//            dependencies.mockBettingService.Setup(bs => bs.Bet(amount)).Returns(() => new BetResult { Type = BetResultType.Loss });

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockBalanceManager.Verify(bm => bm.Withdraw(It.Is<decimal>(d => d == amount)), Times.Once);
//            dependencies.mockNotifier.Verify(n => n(string.Format(BET_LOSS_MESSAGE, balance)), Times.Once);
//        }

//        [Test]
//        public void Start_ShouldHandleBetCommandAndExitSuccessfullyWhenBetAmountIsHigherThanThreshold()
//        {
//            var amount = 15;
//            var dependencies = GetMockedDependencies();

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput($"bet {amount}"))
//                           .Returns(CreateExitCommand());

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockNotifier.Verify(n => n(AMOUNT_OUT_OF_RANGE_MESSAGE), Times.Once);
//        }

//        [Test]
//        public void Start_ShouldHandleBetCommandAndExitSuccessfullyWhenBetAmountIsLowerThanThreshold()
//        {
//            var amount = 0.5m;
//            var dependencies = GetMockedDependencies();

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput($"bet {amount}"))
//                           .Returns(CreateExitCommand());

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockNotifier.Verify(n => n(AMOUNT_OUT_OF_RANGE_MESSAGE), Times.Once);
//        }

//        [Test]
//        public void Start_ShouldNotifyInvalidArgumentIfCommandIsNotExitAndThereAreLessThanTwoArguments()
//        {
//            var dependencies = GetMockedDependencies();
//            var operation = "bet";

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput($"{operation}"))
//                           .Returns(CreateExitCommand());

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();

//            dependencies.mockNotifier.Verify(n => n(string.Format(INVALID_ARGUMENT_OPERATION_MESSAGE, operation)), Times.Once);
//        }

//        [Test]
//        public void Start_ShouldHandleUnexpectedExceptions()
//        {
//            var dependencies = GetMockedDependencies();
//            var operation = "bet";
//            var errorMessage = "test";

//            dependencies.mockInputReader.SetupSequence(reader => reader.Read())
//                           .Returns(CreateCommandInput($"bet 10"))
//                           .Returns(CreateExitCommand());

//            dependencies.mockCommandFactory.Setup(cf => cf.Create(It.IsAny<string[]>())).Throws(() => new Exception(errorMessage));

//            var bettingGame = CreateGame(dependencies);

//            bettingGame.Start();
//            dependencies.mockNotifier.Verify(n => n($"Something went wrong on our end, please try again! {errorMessage}"), Times.Once);
//        }

//        private (Mock<ICommandFactory> mockCommandFactory, Mock<IInputReader> mockInputReader, Mock<Action<string>> mockNotifier) GetMockedDependencies()
//        {
//            return (new Mock<ICommandFactory>(), new Mock<IInputReader>(), new Mock<Action<string>>());
//        }

//        private string[] CreateExitCommand()
//            => CreateCommandInput("exit");

//        private string[] CreateCommandInput(string command) => new string[] { command };

//        private IGame CreateGame((Mock<ICommandFactory> mockCommandFactory, Mock<IInputReader> mockInputReader, Mock<Action<string>> mockNotifier) dependencies)
//        {
//            return new BettingGame(dependencies.mockCommandFactory.Object, dependencies.mockInputReader.Object, dependencies.mockNotifier.Object);
//        }
//    }
//}
