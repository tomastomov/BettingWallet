using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation;
using BettingWallet.Core.Implementation.Betting;
using Moq;
using NUnit.Framework;

namespace BettingWallet.Tests.Betting
{
    public class BettingServiceTests
    {
        [Test]
        public void Bet_ShouldPlaceBetAndReturnLossResultCorrectly()
        {
            var dependencies = GetDependencies();
            var amount = 10;

            dependencies.mockRandomGenerator.Setup(rg => rg.GenerateOutcome(It.IsAny<int>(), It.IsAny<int>())).Returns(Outcome.Loss);

            var bettingService = new BettingService(dependencies.mockRandomGenerator.Object, dependencies.mockEarningsCalculator.Object);

            var result = bettingService.Bet(amount);

            Assert.AreEqual(BetResultType.Loss, result.Type);
        }

        [Test]
        public void Bet_ShouldPlaceBetAndReturnUpToTwoTimesWinCorrectly()
        {
            var dependencies = GetDependencies();
            var integerCoefficient = 1;
            var fractionalCoefficient = 0.25m;
            var amount = 10;
            var winnings = amount * (integerCoefficient + fractionalCoefficient);

            dependencies.mockRandomGenerator.Setup(rg => rg.GenerateOutcome(It.IsAny<int>(), It.IsAny<int>())).Returns(Outcome.WinUpToTwoTimes);
            dependencies.mockRandomGenerator.Setup(rg => rg.GenerateCoefficient(1, 2)).Returns(integerCoefficient);
            dependencies.mockRandomGenerator.Setup(rg => rg.GenerateFractionalCoefficient()).Returns(fractionalCoefficient);
            dependencies.mockEarningsCalculator.Setup(ec => ec.Calculate(amount, 1, 2)).Returns(winnings);

            var bettingService = new BettingService(dependencies.mockRandomGenerator.Object, dependencies.mockEarningsCalculator.Object);

            var result = bettingService.Bet(amount);

            Assert.AreEqual(BetResultType.Win, result.Type);
            Assert.AreEqual(winnings, result.AmountWon);
        }

        private (Mock<IRandomGenerator> mockRandomGenerator, Mock<IEarningsCalculator> mockEarningsCalculator) GetDependencies()
            => (new Mock<IRandomGenerator>(), new Mock<IEarningsCalculator>());
    }
}
