namespace BettingWallet.Core.Implementation.Commands
{
    public record CommandExecutionContext
    {
        public decimal Amount { get; init; }
    }
}
