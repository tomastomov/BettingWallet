using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation.Betting;
using BettingWallet.Core.Implementation.Commands;
using Moq;
using NUnit.Framework;
using System;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Tests.Commands
{
    public class BetCommandTests
    {
        private Mock<IBalanceManager> _mockBalanceManager;
        private Mock<IBettingService> _mockBettingService;
        private Mock<Action<string>> _mockNotify;
        private decimal _balance = 20;

        [SetUp]
        public void Setup()
        {
            _mockBalanceManager = new Mock<IBalanceManager>();
            _mockBettingService = new Mock<IBettingService>();
            _mockNotify = new Mock<Action<string>>();

            _mockBalanceManager.SetupGet(x => x.Balance).Returns(_balance);
        }

        [Test]
        public void Execute_ShouldNotifyAmountOutOfRangeWhenAmountIsTooLow()
        {
            var command = CreateCommand(0);

            command.Execute();

            _mockNotify.Verify(n => n.Invoke(AMOUNT_OUT_OF_RANGE_MESSAGE), Times.Once());
            _mockBalanceManager.Verify(bm => bm.Withdraw(It.IsAny<decimal>()), Times.Never());
            _mockBettingService.Verify(bs => bs.Bet(It.IsAny<decimal>()), Times.Never());
        }

        [Test]
        public void Execute_ShouldNotifyAmountOutOfRangeWhenAmountIsTooHigh()
        {
            var command = CreateCommand(15);

            command.Execute();

            _mockNotify.Verify(n => n.Invoke(AMOUNT_OUT_OF_RANGE_MESSAGE), Times.Once());
            _mockBalanceManager.Verify(bm => bm.Withdraw(It.IsAny<decimal>()), Times.Never());
            _mockBettingService.Verify(bs => bs.Bet(It.IsAny<decimal>()), Times.Never());
        }

        [Test]
        public void Execute_NotifyWinAndDepositAmountWonWhenBetIsWon()
        {
            var amount = 10;
            var amountWon = 15;

            _mockBettingService.Setup(bs => bs.Bet(amount)).Returns(new BetResult { Type = BetResultType.Win, AmountWon = amountWon });

            var command = CreateCommand(amount);
            command.Execute();

            _mockBalanceManager.Verify(bm => bm.Withdraw(amount), Times.Once());
            _mockBalanceManager.Verify(bm => bm.Deposit(amountWon), Times.Once());
            _mockNotify.Verify(n => n(string.Format(BET_WIN_MESSAGE, amountWon, _balance)));
        }

        [Test]
        public void Execute_ShouldNotifyLossWhenBetResultIsLoss()
        {
            var betResult = new BetResult { Type = BetResultType.Loss };
            _mockBettingService.Setup(bs => bs.Bet(10)).Returns(betResult);

            var command = CreateCommand(10);
            command.Execute();

            _mockNotify.Verify(n => n.Invoke(string.Format(BET_LOSS_MESSAGE, _balance)), Times.Once());
            _mockBalanceManager.Verify(bm => bm.Deposit(It.IsAny<decimal>()), Times.Never());
        }

        private ICommand CreateCommand(decimal amount)
        {
            return new BetCommand(_mockBalanceManager.Object, _mockBettingService.Object, _mockNotify.Object, amount);
        }
    }
}
