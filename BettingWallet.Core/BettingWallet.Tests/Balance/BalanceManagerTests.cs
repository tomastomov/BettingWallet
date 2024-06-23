using BettingWallet.Core.Implementation;
using NUnit.Framework;
using System;

namespace BettingWallet.Tests.Balance
{
    public class BalanceManagerTests
    {
        [Test]
        public void Add_ShouldIncreaseTheBalanceCorrectly()
        {
            var balanceManager = new BalanceManager();
            var amount = 10;

            balanceManager.Add(amount);

            Assert.AreEqual(amount, balanceManager.Balance);
        }

        [Test]
        public void Add_ShouldThrowIfAmountIsNegative()
        {
            var balanceManager = new BalanceManager();
            var amount = -10;

            Assert.Throws<InvalidOperationException>(() => balanceManager.Add(amount));
        }

        [Test]
        public void Subtract_ShouldDecreaseAmountCorrectlyIfThereIsEnoughBalance()
        {
            var balanceManager = new BalanceManager();
            var amount = 10;
            var balanceAmount = 20;

            balanceManager.Add(balanceAmount);
            balanceManager.Subtract(amount);

            Assert.AreEqual(balanceAmount - amount, balanceManager.Balance);
        }

        [Test]
        public void Subtract_ShouldThrowIfAmountIsBiggerThanBalance()
        {
            var balanceManager = new BalanceManager();
            var amount = 10;

            Assert.Throws<InvalidOperationException>(() => balanceManager.Subtract(amount));
        }

        [Test]
        public void Subtract_ShouldThrowIfAmountIsNegative()
        {
            var balanceManager = new BalanceManager();
            var amount = -10;

            Assert.Throws<InvalidOperationException>(() => balanceManager.Add(amount));
        }
    }
}
