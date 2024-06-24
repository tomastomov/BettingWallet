namespace BettingWallet.Core.Contracts
{
    public interface ICommandFactory
    {
        ICommand Create(string[] parameters);
    }
}
