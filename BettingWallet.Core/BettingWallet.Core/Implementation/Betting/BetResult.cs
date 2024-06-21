namespace BettingWallet.Core.Implementation.Betting
{
    public record BetResult
    {
        public BetResultType Type { get; init; }

        public decimal? AmountWon { get; init; }
    }
}
