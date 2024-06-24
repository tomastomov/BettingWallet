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
            var mockRandomGenerator = new Mock<IRandomGenerator>();
            int lower = 1;
            int upper = 2;
            var amount = 10;
            var integerCoefficient = 1;
            var fractionalCoefficient = 0.25m;
            mockRandomGenerator.Setup(og => og.GenerateCoefficient(lower, upper)).Returns(integerCoefficient);
            mockRandomGenerator.Setup(og => og.GenerateFractionalCoefficient()).Returns(fractionalCoefficient);

            var earningsCalculator = new BetEarningsCalculator(mockRandomGenerator.Object);

            var result = earningsCalculator.Calculate(amount, lower, upper);

            Assert.AreEqual(amount * (integerCoefficient + fractionalCoefficient), result);
        }
    }
}
