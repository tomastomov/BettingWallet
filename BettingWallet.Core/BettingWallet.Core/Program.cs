using BettingWallet.Core.Implementation;
using BettingWallet.Core.Implementation.Betting;

var oddsGenerator = new OddsGenerator(new Random());
var bettingService = new BettingService(oddsGenerator, new BetEarningsCalculator(oddsGenerator));
var game = new BettingGame(new BalanceManager(), bettingService, new ConsoleReader(), (message) => Console.WriteLine(message));

game.Start();