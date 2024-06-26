using BettingWallet.Core.Implementation.Commands;
using Moq;
using NUnit.Framework;
using System;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Tests.Commands
{
    public class InvalidCommandTests
    {
        private Mock<Action<string>> _mockNotify;

        [SetUp]
        public void Setup()
        {
            _mockNotify = new Mock<Action<string>>();
        }

        [Test]
        public void ExecuteShould_NotifyWithErrorMessage()
        {
            var errorMessage = "test";
            var command = new InvalidCommand(_mockNotify.Object, errorMessage);

            command.Execute();

            _mockNotify.Verify(n => n(errorMessage), Times.Once);
        }
    }
}
