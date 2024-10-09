using SimplifiedLotteryGame.Interfaces;
using SimplifiedLotteryGame.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedLotteryGame.Repositories
{
	public class PlayerRepository : IPlayerRepository
	{
		private readonly ConcurrentDictionary<int, Player> _players = new ConcurrentDictionary<int, Player>();

        public void Add(Player player)
		{
			_players.TryAdd(player.Id, player);
		}

		public void Update(Player player)
		{
			_players[player.Id] = player;
		}

		public void Remove(Player player)
		{
			_players.TryRemove(player.Id, out _);
		}

		public IEnumerable<Player> GetAll()
		{
			return _players.Values.ToList();
		}

		public Player GetById(int id)
		{
			_players.TryGetValue(id, out var player);
			return player;
		}


    }
}
