using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation.Commands;
using Moq;
using NUnit.Framework;
using System;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Tests.Commands
{
    public class DepositCommandTests
    {
        private Mock<IBalanceManager> _mockBalanceManager;
        private Mock<Action<string>> _mockNotify;

        [SetUp]
        public void Setup()
        {
            _mockBalanceManager = new Mock<IBalanceManager>();
            _mockNotify = new Mock<Action<string>>();
        }

        [Test]
        public void ExecuteShould_DepositCorrectly()
        {
            var balance = 500m;
            var amount = 100m;
            _mockBalanceManager.SetupGet(x => x.Balance).Returns(balance);

            var command = new DepositCommand(_mockBalanceManager.Object, _mockNotify.Object, amount);
            command.Execute();

            _mockBalanceManager.Verify(m => m.Deposit(amount), Times.Once());
            _mockNotify.Verify(n => n(string.Format(DEPOSIT_MESSAGE, amount, balance)), Times.Once());
        }
    }
}
