using Microsoft.Extensions.DependencyInjection;
using SimplifiedLotteryGame.Helpers;
using SimplifiedLotteryGame.Interfaces;
using SimplifiedLotteryGame.Models;
using SimplifiedLotteryGame.Repositories;
using SimplifiedLotteryGame.Services;

var serviceProvider = new ServiceCollection()
			 .AddSingleton<IPlayerRepository, PlayerRepository>()
             .AddSingleton<IRandomNumberGenerator, GenerateRandomNumber>()
             .AddScoped<ILotteryService, LotteryService>()
			 .BuildServiceProvider();

	var lotteryService = serviceProvider.GetRequiredService<ILotteryService>();

	var user = new Player { Id = 1 };
	lotteryService.AddPlayer(user);

	Console.WriteLine($"Welcome Player {user.Id}");
	Console.WriteLine($"Digital balance: {user.Money}");
	Console.WriteLine($"Ticket price: $1");
	bool playAgain = true;

	while (playAgain && user.Money > 0)
	{
		Console.WriteLine("How many tickets would you like to purchase? (1-10)");
		var chosenTickets = Console.ReadLine();
		var ticketCount = HelperMethods.SanitizeInput(chosenTickets);

		if (ticketCount.HasValue)
		{
			lotteryService.PurchaseTickets(user, ticketCount.Value);
			lotteryService.RunLottery();

			if (!lotteryService.UserHasMoney(user))
			{
				Console.WriteLine("You're out of money. Game over.");
				break;
			}
		
			Console.WriteLine($"Digital balance: {user.Money}");
			Console.WriteLine("Do you want to play again? (yes/no)");
			var response = Console.ReadLine().ToLower();

			if (response == "yes")	
					playAgain = true;	
				else	
					playAgain = false;	
			}
			else
			{
				Console.WriteLine("Invalid input, please try again.");
			}
	}

	Console.WriteLine("Thanks for playing!");
        


