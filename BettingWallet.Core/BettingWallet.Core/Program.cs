using BettingWallet.Core.Implementation;
using BettingWallet.Core.Implementation.Betting;

var notifier = (string message) =>
{
    Console.WriteLine(message);
};

var balanceManager = new BalanceManager();
var oddsGenerator = new OddsGenerator(new Random());
var bettingService = new BettingService(oddsGenerator, new BetEarningsCalculator(oddsGenerator));
var game = new BettingGame(balanceManager, bettingService, notifier);

game.Start();