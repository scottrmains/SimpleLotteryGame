using SimplifiedLotteryGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedLotteryGame.Interfaces
{
	public interface IPlayerRepository
	{
		void Add(Player player);
		void Update(Player player);
		void Remove(Player player);
		IEnumerable<Player> GetAll();
		Player GetById(int id);
	}
}
