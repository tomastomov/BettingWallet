using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation.Betting;
using Moq;
using NUnit.Framework;

namespace BettingWallet.Tests.Betting
{
    public class BettingServiceTests
    {
        [Test]
        public void Bet_ShouldPlaceBetAndReturnWinAmountCorrectly()
        {
            var dependencies = GetDependencies();
            var odd = 80;
            var amount = 10;
            var winnings = 18;

            dependencies.mockOddsGenerator.Setup(og => og.Generate(It.IsAny<int>(), It.IsAny<int>())).Returns(odd);

            dependencies.mockEarningsCalculator.Setup(ec => ec.Calculate(amount, odd)).Returns(winnings);

            var bettingService = new BettingService(dependencies.mockOddsGenerator.Object, dependencies.mockEarningsCalculator.Object);

            var result = bettingService.Bet(amount);

            Assert.AreEqual(winnings, result.AmountWon);
            Assert.AreEqual(BetResultType.Win, result.Type);
        }

        [Test]
        public void Bet_ShouldPlaceBetAndReturnLossResultCorrectly()
        {
            var dependencies = GetDependencies();

            dependencies.mockOddsGenerator.Setup(og => og.Generate(It.IsAny<int>(), It.IsAny<int>())).Returns(10);

            var bettingService = new BettingService(dependencies.mockOddsGenerator.Object, dependencies.mockEarningsCalculator.Object);

            var result = bettingService.Bet(10);

            Assert.AreEqual(BetResultType.Loss, result.Type);
            Assert.Null(result.AmountWon);
        }

        private (Mock<IOddsGenerator> mockOddsGenerator, Mock<IEarningsCalculator> mockEarningsCalculator) GetDependencies()
            => (new Mock<IOddsGenerator>(), new Mock<IEarningsCalculator>());
    }
}
