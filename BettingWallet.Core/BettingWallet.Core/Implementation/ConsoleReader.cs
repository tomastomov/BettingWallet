using BettingWallet.Core.Contracts;

namespace BettingWallet.Core.Implementation
{
    public class ConsoleReader : IInputReader
    {
        public string[] Read()
        {
            return Console.ReadLine()?.ToLower().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>(); ;
        }
    }
}
