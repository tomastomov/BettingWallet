using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation;
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
            var mockBalanceManager = new Mock<IBalanceManager>();
            var mockBettingService = new Mock<IBettingService>();
            var mockInputReader = new Mock<IInputReader>();
            var mockNotifier = new Mock<Action<string>>();

            mockInputReader.Setup(ir => ir.ReadLine()).Returns("exit");

            var bettingGame = new BettingGame(mockBalanceManager.Object, mockBettingService.Object, mockInputReader.Object, mockNotifier.Object);

            bettingGame.Start();

            mockNotifier.Verify(n => n(EXIT_MESSAGE), Times.Once);
        }

        [Test]
        public void Start_ShouldProcessDepositCommand()
        {
            var amount = 100;
            var mockBalanceManager = new Mock<IBalanceManager>();
            var mockBettingService = new Mock<IBettingService>();
            var mockInputReader = new Mock<IInputReader>();
            var mockNotifier = new Mock<Action<string>>();

            mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns($"deposit {amount}")
                           .Returns("exit");

            var bettingGame = new BettingGame(mockBalanceManager.Object, mockBettingService.Object, mockInputReader.Object, mockNotifier.Object);

            bettingGame.Start();

            mockBalanceManager.Verify(bm => bm.Add(It.Is<decimal>(d => d == amount)), Times.Once);
        }

        [Test]
        public void Start_ShouldNotifyUnsupportedCommand()
        {
            var mockBalanceManager = new Mock<IBalanceManager>();
            var mockBettingService = new Mock<IBettingService>();
            var mockInputReader = new Mock<IInputReader>();
            var mockNotifier = new Mock<Action<string>>();

            mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns("unsupported 10")
                           .Returns("exit");

            var bettingGame = new BettingGame(mockBalanceManager.Object, mockBettingService.Object, mockInputReader.Object, mockNotifier.Object);

            bettingGame.Start();

            mockNotifier.Verify(n => n(UNSUPPORTED_COMMAND_MESSAGE), Times.Once);
        }

        [Test]
        public void Start_ShouldHandleInvalidOperationException()
        {
            var mockBalanceManager = new Mock<IBalanceManager>();
            var mockBettingService = new Mock<IBettingService>();
            var mockInputReader = new Mock<IInputReader>();
            var mockNotifier = new Mock<Action<string>>();

            mockInputReader.SetupSequence(reader => reader.ReadLine())
                           .Returns("bet abc")
                           .Returns("exit");

            var bettingGame = new BettingGame(mockBalanceManager.Object, mockBettingService.Object, mockInputReader.Object, mockNotifier.Object);

            bettingGame.Start();

            mockNotifier.Verify(n => n(It.Is<string>(msg => msg.Contains("Something went wrong on our end"))), Times.Once);
        }
    }
}
