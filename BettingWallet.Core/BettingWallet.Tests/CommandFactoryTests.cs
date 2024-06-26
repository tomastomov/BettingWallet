using BettingWallet.Core.Contracts;
using BettingWallet.Core.Implementation;
using BettingWallet.Core.Implementation.Commands;
using Moq;
using NUnit.Framework;
using System;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Tests
{
    public class CommandFactoryTests
    {
        private ICommandFactory _commandFactory;
        private Mock<IBalanceManager> _mockBalanceManager;
        private Mock<IBettingService> _mockBettingService;
        private Mock<Action<string>> _mockNotify;

        [SetUp]
        public void Setup()
        {
            _mockBalanceManager = new Mock<IBalanceManager>();
            _mockBettingService = new Mock<IBettingService>();
            _mockNotify = new Mock<Action<string>>();
            _commandFactory = new CommandFactory(_mockBalanceManager.Object, _mockBettingService.Object, _mockNotify.Object);
        }

        [Test]
        public void Create_ShouldReturnDepositCommandWhenParametersAreValid()
        {
            var parameters = new[] { DEPOSIT, "100" };

            var command = _commandFactory.Create(parameters);

            Assert.IsInstanceOf<DepositCommand>(command);
        }

        [Test]
        public void Create_ShouldReturnBetCommandWhenParametersAreValid()
        {
            var parameters = new[] { BET, "50" };

            var command = _commandFactory.Create(parameters);

            Assert.IsInstanceOf<BetCommand>(command);
        }

        [Test]
        public void Create_ShouldReturnWithdrawCommandWhenParametersAreValid()
        {
            var parameters = new[] { WITHDRAW, "50" };

            var command = _commandFactory.Create(parameters);

            Assert.IsInstanceOf<WithdrawCommand>(command);
        }

        [Test]
        public void Create_ShouldReturnInvalidCommandWhenAmountIsInvalid()
        {
            var parameters = new[] { WITHDRAW, "invalid_amount" };

            var command = _commandFactory.Create(parameters);

            Assert.IsInstanceOf<InvalidCommand>(command);
        }

        [Test]
        public void Create_ShouldReturnInvalidCommandWhenTooManyParameters()
        {
            var parameters = new[] { DEPOSIT, "100", "extra_param" };

            var command = _commandFactory.Create(parameters);

            Assert.IsInstanceOf<InvalidCommand>(command);
        }

        [Test]
        public void Create_ShouldReturnExitCommandWhenCommandIsExit()
        {
            var parameters = new[] { EXIT };

            var command = _commandFactory.Create(parameters);

            Assert.IsInstanceOf<ExitCommand>(command);
        }

        [Test]
        public void Create_ShouldReturnUnsupportedCommandWhenCommandIsUnsupported()
        {
            var parameters = new[] { "unsupported_command" };

            var command = _commandFactory.Create(parameters);

            Assert.IsInstanceOf<InvalidCommand>(command);
        }
    }
}
