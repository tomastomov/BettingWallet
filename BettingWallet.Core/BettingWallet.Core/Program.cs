decimal balance = 0m;

while (true)
{
    Console.WriteLine("Please, submit action:");

    var command = Console.ReadLine()?.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
    var random = new Random();


    if (command == null ||  command.Length == 0)
    {
        Console.WriteLine("Invalid command. Supported commands are withdraw, exit, bet, deposit");
        continue;
    }

    var commandType = command[0];
    var amount = command.Length > 1 ? decimal.Parse(command[1]) : 0;

    if (amount < 0)
    {
        Console.WriteLine("You must provide a positive amount!");
        continue;
    }

    switch (commandType)
    {
        case "exit":
            Console.WriteLine("Thank you for playing! Hope to see you again soon.");
            return;
        case "deposit":
            if (command.Length < 2)
            {
                Console.WriteLine("In order to deposit, you must provide a valid amount");
                continue;
            }
            balance += amount;
            Console.WriteLine($"Your deposit of ${amount} was successful. Your current balance is ${balance}");
            break;
        case "bet":
            if (command.Length < 2)
            {
                Console.WriteLine("In order to bet, you must provide a valid amount");
                continue;
            }
            if (amount < 1 || amount > 10)
            {
                Console.WriteLine("The allowed betting range is $1-$10.");
                continue;
            }
            if (amount > balance)
            {
                Console.WriteLine("Insufficient funds to bet.");
                continue;
            }
            balance -= amount;
            // 1 - 5 -> loss, 6 -> 9 -> 2x, 10 -> 2x-10x
            var outcome = random.Next(1, 10);
            var multiplierLow = 1;
            var multiplierHigh = 2; 
            if (outcome >= 1 && outcome <= 5)
            {
                Console.WriteLine($"No luck this time! Your current balance is: ${balance}");
                continue;
            } else if (outcome == 10)
            {
                multiplierLow = 2;
                multiplierHigh = 10;
            }
            var winMultiplierBase = random.Next(multiplierLow, multiplierHigh);
            var winMultiplierFraction = NextDecimal(random);
            if (winMultiplierBase == multiplierHigh)
            {
                winMultiplierFraction = 0;
            }
            var winMultiplier = winMultiplierBase + winMultiplierFraction;
            var winAmount = amount * winMultiplier;
            balance += winAmount;
            Console.WriteLine($"Congrats - you won ${winAmount}! Your current balance is: ${balance}");
            break;
        case "withdraw":
            if (command.Length < 2)
            {
                Console.WriteLine($"In order to perform this {commandType}, you must provide a valid amount");
                continue;
            }
            if (amount > balance)
            {
                Console.WriteLine("Cannot withdraw more than you have");
                continue;
            }

            balance -= amount;
            Console.WriteLine($"Your withdrawal of ${amount} was successful. Your current balance is ${balance}");
            break;
        default:
            break;
    }
}

decimal NextDecimal(Random rng)
{
    return new decimal(rng.NextDouble());
}

