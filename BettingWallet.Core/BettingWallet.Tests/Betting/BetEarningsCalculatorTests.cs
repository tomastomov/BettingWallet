using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation.Betting;
using Moq;
using NUnit.Framework;

namespace BettingWallet.Tests.Betting
{
    public class BetEarningsCalculatorTests
    {
        [Test]
        public void Calculate_ShouldReturnTheAmountWonCorrectly()
        {
            var mockOddsGenerator = new Mock<IOddsGenerator>();
            mockOddsGenerator.Setup(og => og.GenerateFraction()).Returns(0.25m);

            var earningsCalculator = new BetEarningsCalculator(mockOddsGenerator.Object);

            var result = earningsCalculator.Calculate(10, 5);

            Assert.AreEqual(11.25, result);
        }
    }
}
