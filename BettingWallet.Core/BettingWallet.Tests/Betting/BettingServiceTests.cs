using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation;
using BettingWallet.Core.Implementation.Betting;
using Moq;
using NUnit.Framework;

namespace BettingWallet.Tests.Betting
{
    public class BettingServiceTests
    {
        private Mock<IRandomGenerator> _mockRandomGenerator;
        private Mock<IEarningsCalculator> _mockEarningsCalculator;

        private IBettingService _bettingService;

        [SetUp]
        public void Setup()
        {
            _mockRandomGenerator = new Mock<IRandomGenerator>();
            _mockEarningsCalculator = new Mock<IEarningsCalculator>();
            _bettingService = new BettingService(_mockRandomGenerator.Object, _mockEarningsCalculator.Object);
        }
        

        [Test]
        public void Bet_ShouldPlaceBetAndReturnLossResultCorrectly()
        {
            var amount = 10;

            _mockRandomGenerator.Setup(rg => rg.GenerateOutcome(It.IsAny<int>(), It.IsAny<int>())).Returns(Outcome.Loss);

            var result = _bettingService.Bet(amount);

            Assert.AreEqual(BetResultType.Loss, result.Type);
        }

        [Test]
        public void Bet_ShouldPlaceBetAndReturnUpToTwoTimesWinCorrectly()
        {
            var integerCoefficient = 1;
            var fractionalCoefficient = 0.25m;
            var amount = 10;
            var winnings = amount * (integerCoefficient + fractionalCoefficient);

            _mockRandomGenerator.Setup(rg => rg.GenerateOutcome(It.IsAny<int>(), It.IsAny<int>())).Returns(Outcome.WinUpToTwoTimes);
            _mockRandomGenerator.Setup(rg => rg.GenerateCoefficient(1, 2)).Returns(integerCoefficient);
            _mockRandomGenerator.Setup(rg => rg.GenerateFractionalCoefficient()).Returns(fractionalCoefficient);
            _mockEarningsCalculator.Setup(ec => ec.Calculate(amount, 1, 2)).Returns(winnings);

            var result = _bettingService.Bet(amount);

            Assert.AreEqual(BetResultType.Win, result.Type);
            Assert.AreEqual(winnings, result.AmountWon);
        }

        [Test]
        public void Bet_ShouldPlaceBetAndReturnUpToTenTimesWinCorrectly()
        {
            var integerCoefficient = 2;
            var fractionalCoefficient = 0.25m;
            var amount = 10;
            var winnings = amount * (integerCoefficient + fractionalCoefficient);

            _mockRandomGenerator.Setup(rg => rg.GenerateOutcome(It.IsAny<int>(), It.IsAny<int>())).Returns(Outcome.WinUpToTenTimes);
            _mockRandomGenerator.Setup(rg => rg.GenerateCoefficient(1, 2)).Returns(integerCoefficient);
            _mockRandomGenerator.Setup(rg => rg.GenerateFractionalCoefficient()).Returns(fractionalCoefficient);
            _mockEarningsCalculator.Setup(ec => ec.Calculate(amount, 2, 10)).Returns(winnings);

            var result = _bettingService.Bet(amount);

            Assert.AreEqual(BetResultType.Win, result.Type);
            Assert.AreEqual(winnings, result.AmountWon);
        }

        [Test]
        public void Bet_ShouldPlaceBetAndReturnTenTimesWinCorrectly()
        {
            var integerCoefficient = 10;
            var fractionalCoefficient = 0m;
            var amount = 10;
            var winnings = amount * (integerCoefficient + fractionalCoefficient);

            _mockRandomGenerator.Setup(rg => rg.GenerateOutcome(It.IsAny<int>(), It.IsAny<int>())).Returns(Outcome.WinUpToTenTimes);
            _mockRandomGenerator.Setup(rg => rg.GenerateCoefficient(1, 2)).Returns(integerCoefficient);
            _mockEarningsCalculator.Setup(ec => ec.Calculate(amount, 2, 10)).Returns(winnings);

            var result = _bettingService.Bet(amount);

            Assert.AreEqual(BetResultType.Win, result.Type);
            Assert.AreEqual(winnings, result.AmountWon);
        }
    }
}
