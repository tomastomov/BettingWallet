using BettingWallet.Core.Implementation.Commands;
using Moq;
using NUnit.Framework;
using System;
using static BettingWallet.Core.Constants;

namespace BettingWallet.Tests.Commands
{
    public class ExitCommandTests
    {
        private Mock<Action<string>> _mockNotify;

        [SetUp]
        public void Setup()
        {
            _mockNotify = new Mock<Action<string>>();
        }

        [Test]
        public void ExecuteShould_NotifyWithExitMessage()
        {
            var command = new ExitCommand(_mockNotify.Object);

            command.Execute();

            _mockNotify.Verify(n => n(EXIT_MESSAGE), Times.Once);
        }
    }
}
