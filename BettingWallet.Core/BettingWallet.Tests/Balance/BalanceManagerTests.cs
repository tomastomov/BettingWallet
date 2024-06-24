using BettingWallet.Core.Implementation;
using NUnit.Framework;
using System;

namespace BettingWallet.Tests.Balance
{
    public class BalanceManagerTests
    {
        [Test]
        public void Deposit_ShouldIncreaseTheBalanceCorrectly()
        {
            var balanceManager = new BalanceManager();
            var amount = 10;

            balanceManager.Deposit(amount);

            Assert.AreEqual(amount, balanceManager.Balance);
        }

        [Test]
        public void Deposit_ShouldThrowIfAmountIsNegative()
        {
            var balanceManager = new BalanceManager();
            var amount = -10;

            Assert.Throws<InvalidOperationException>(() => balanceManager.Deposit(amount));
        }

        [Test]
        public void Withdraw_ShouldDecreaseAmountCorrectlyIfThereIsEnoughBalance()
        {
            var balanceManager = new BalanceManager();
            var amount = 10;
            var balanceAmount = 20;

            balanceManager.Deposit(balanceAmount);
            balanceManager.Withdraw(amount);

            Assert.AreEqual(balanceAmount - amount, balanceManager.Balance);
        }

        [Test]
        public void Withdraw_ShouldThrowIfAmountIsBiggerThanBalance()
        {
            var balanceManager = new BalanceManager();
            var amount = 10;

            Assert.Throws<InvalidOperationException>(() => balanceManager.Withdraw(amount));
        }

        [Test]
        public void Withdraw_ShouldThrowIfAmountIsNegative()
        {
            var balanceManager = new BalanceManager();
            var amount = -10;

            Assert.Throws<InvalidOperationException>(() => balanceManager.Deposit(amount));
        }
    }
}
