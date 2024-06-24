using BettingWallet.Core.Implementation;
using NUnit.Framework;

namespace BettingWallet.Tests
{
    public class RandomGeneratorTests
    {
        [Test]
        public void GenerateCoefficient_ShouldReturnAnInteger()
        {
            var randomNumber = 5;
            var mockRandom = new MockRandom();
            mockRandom.DepositInt(randomNumber);

            var oddsGenerator = new RandomGenerator(mockRandom);
            var result = oddsGenerator.GenerateCoefficient(1, 10);

            Assert.AreEqual(randomNumber, result);
        }

        [Test]
        public void GenerateFractionCoefficient_ShouldReturnADecimalBetweenZeroAndOne()
        {
            var randomNumber = 25;
            var mockRandom = new MockRandom();
            mockRandom.DepositInt(randomNumber);

            var oddsGenerator = new RandomGenerator(mockRandom);
            var result = oddsGenerator.GenerateFractionalCoefficient();

            Assert.AreEqual(randomNumber / 100m, result);
            Assert.True(result >= 0.0m && result <= 1.0m);
        }
    }
}
