using BettingWallet.Core.Contracts;

namespace BettingWallet.Core.Implementation
{
    public class ConsoleReader : IInputReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
