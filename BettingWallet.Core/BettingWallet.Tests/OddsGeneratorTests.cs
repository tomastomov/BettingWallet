using BettingWallet.Core.Implementation;
using NUnit.Framework;

namespace BettingWallet.Tests
{
    public class OddsGeneratorTests
    {
        [Test]
        public void Generate_ShouldReturnAnInteger()
        {
            var randomNumber = 5;
            var mockRandom = new MockRandom();
            mockRandom.AddInt(randomNumber);

            var oddsGenerator = new OddsGenerator(mockRandom);
            var result = oddsGenerator.Generate(1, 10);

            Assert.AreEqual(randomNumber, result);
        }

        [Test]
        public void GenerateFraction_ShouldReturnADecimal()
        {
            var randomNumber = 5;
            var mockRandom = new MockRandom();
            mockRandom.AddDouble(randomNumber);

            var oddsGenerator = new OddsGenerator(mockRandom);
            var result = oddsGenerator.GenerateFraction();

            Assert.AreEqual(randomNumber, result);
        }
    }
}
