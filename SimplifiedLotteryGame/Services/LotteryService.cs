using SimplifiedLotteryGame.Helpers;
using SimplifiedLotteryGame.Interfaces;
using SimplifiedLotteryGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimplifiedLotteryGame.Services
{
    public class LotteryService : ILotteryService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IRandomNumberGenerator _randomGenerator;

        public int houseProfit { get; private set; } = 0;

        public LotteryService(IPlayerRepository playerRepository, IRandomNumberGenerator randomGenerator)
        {
            _playerRepository = playerRepository;
            _randomGenerator = randomGenerator;
        }

        public void AddPlayer(Player player)
        {
            _playerRepository.Add(player);
        }

        public void PurchaseTickets(Player player, int ticketCount)
        {

            if (!player.HasMoney)
            {
                Console.WriteLine("Player cannot afford any more tickets.");
                return;
            }

            int affordableTickets = Math.Min(ticketCount, player.Money);

            var tickets = new List<Ticket>();
            for (int i = 0; i < affordableTickets; i++)
            {
                tickets.Add(new Ticket());
          
            }

            player.PurchaseTickets(tickets);
            _playerRepository.Update(player);
        }

        /// <summary>
        /// Main lottery function
        /// </summary>
        public void RunLottery()
        {


            EnsureMinimumPlayers();

            var players = _playerRepository.GetAll().ToList();
            var totalRevenue = players.Sum(p => p.Tickets.Sum(t => t.Price));
            Console.WriteLine($"Total revenue: ${totalRevenue}");

            DistributePrizes(players, totalRevenue);
            RemoveBrokePlayers(players);
            ResetTickets();
        }


        public void ResetTickets()
        {
            var players = _playerRepository.GetAll().ToList();
            foreach (var player in players)
            {
                player.Tickets.Clear(); 
                _playerRepository.Update(player); 
            }
        }


        /// <summary>
        /// Ensures theres enough players and that theyve bought tickets
        /// </summary>
        private void EnsureMinimumPlayers()
        {
            var players = _playerRepository.GetAll();
            int requiredPlayers = 10 + _randomGenerator.Next(6);

            for (int i = players.Count(); i < requiredPlayers; i++)
            {
                var botPlayer = new Player { Id = i + 1 };
                AddPlayer(botPlayer);

            }
            players = _playerRepository.GetAll();
            foreach (var player in players.Where(x => x.Id != 1)) 
            {
                int botTickets = _randomGenerator.Next(1, Math.Min(11, player.Money + 1));
                PurchaseTickets(player, botTickets);
                Console.WriteLine($"Player {player.Id} enters the game with {botTickets} tickets.");
            }
        }

        private void DistributePrizes(IEnumerable<Player> players, int totalRevenue)
        {

            int grandPrize = (int)(totalRevenue * 0.50);
            Console.WriteLine($"Grand prize is {grandPrize}");
            int secondPlacePrize = (int)(totalRevenue * 0.30);
            Console.WriteLine($"Second place prize is {secondPlacePrize}");
            int thirdPlacePrize = (int)(totalRevenue * 0.10);
            Console.WriteLine($"Third place prize is {thirdPlacePrize}");

            var ticketToPlayer = players
                .SelectMany(p => p.Tickets.Select(t => new { Ticket = t, Player = p }))
                .ToDictionary(tp => tp.Ticket, tp => tp.Player);

            var allTickets = ticketToPlayer.Keys.ToList();

            var grandPrizeWinner = SelectGrandPrizeWinner(allTickets, ticketToPlayer);
            DistributePrize("Grand Prize", grandPrize, new List<Player> { grandPrizeWinner });  

            var secondPlaceWinners = SelectWinners(allTickets, ticketToPlayer, 0.10);
            DistributePrize("2nd Place", secondPlacePrize, secondPlaceWinners);

            var thirdPlaceWinners = SelectWinners(allTickets, ticketToPlayer, 0.20);
            DistributePrize("3rd Place", thirdPlacePrize, thirdPlaceWinners);


            Console.WriteLine($"House wins {houseProfit}");
        }

        private Player SelectGrandPrizeWinner(List<Ticket> allTickets, Dictionary<Ticket, Player> ticketToPlayer)
        {
            var selectedTicket = allTickets[_randomGenerator.Next(allTickets.Count)];
            var grandPrizeWinner = ticketToPlayer[selectedTicket];
            allTickets.Remove(selectedTicket);

            return grandPrizeWinner;
        }


        /// <summary>
        /// Selects a list of unique winners based on the percentage of remaining tickets.
        /// </summary>
        /// <param name="allTickets">List of remaining tickets.</param>
        /// <param name="ticketToPlayer">Dictionary mapping tickets to players.</param>
        /// <param name="percentage">Percentage of tickets to select as winners.</param>
        /// <returns>List of winning players.</returns>
        private List<Player> SelectWinners(List<Ticket> allTickets, Dictionary<Ticket, Player> ticketToPlayer, double percentage)
        {
            int winnersCount = Math.Max((int)Math.Ceiling(allTickets.Count * percentage), 1);
            var selectedTickets = allTickets
                .OrderBy(t => _randomGenerator.Next(allTickets.Count))
                .Take(winnersCount)
                .ToList();

            var winners = selectedTickets
                .Select(t => ticketToPlayer[t])
                .Distinct()
                .ToList();

            foreach (var ticket in selectedTickets)
            {
                allTickets.Remove(ticket);
            }

            return winners;
        }
        /// <summary>
        /// Distributes the prize amount among the winners and announces them.
        /// </summary>
        /// <param name="place">Prize tier (e.g., "2nd", "3rd").</param>
        /// <param name="prizeAmount">Total prize amount for the tier.</param>
        /// <param name="winners">List of winners.</param>
        /// <returns>Amount added to house profit (remainder).</returns>
        private void DistributePrize(string place, int prizeAmount, List<Player> winners)
        {
            if (winners == null || winners.Count == 0)
            {
                Console.WriteLine($"No winners for the {place} prize.");
                houseProfit += prizeAmount;
                return;
            }

            int splitPrize = prizeAmount / winners.Count;
            int remainder = prizeAmount % winners.Count;

            foreach (var winner in winners)
            {
                winner.Money += splitPrize;
            }

            for (int i = 0; i < remainder; i++)
            {
                winners[i].Money += 1;
            }

            houseProfit += remainder;

            Console.WriteLine($"** Winners for {place} prize: {string.Join(", ", winners.Select(w => $"Player {w.Id}"))}. Each receives ${splitPrize}.");
        }

        private void RemoveBrokePlayers(List<Player> players)
        {
            foreach (var player in players.ToList())  
                if (!player.HasMoney)
                {
                    Console.WriteLine($"Player {player.Id} is out of money and is removed from the game.");
                    _playerRepository.Remove(player);
                }
        }
        

        public bool UserHasMoney(Player user)
        {
            return user.HasMoney;
        }
    }
}
