using BettingWallet.Core.Implementation;
using BettingWallet.Core.Implementation.Betting;

var oddsGenerator = new RandomGenerator(new Random());
var bettingService = new BettingService(oddsGenerator, new BetEarningsCalculator(oddsGenerator));
var commandFactory = new CommandFactory(new BalanceManager(), bettingService, (message) => Console.WriteLine(message));
var game = new BettingGame(commandFactory, new ConsoleReader(), (message) => Console.WriteLine(message));

game.Start();