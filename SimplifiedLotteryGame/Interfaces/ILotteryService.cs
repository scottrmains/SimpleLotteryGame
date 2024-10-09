using SimplifiedLotteryGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedLotteryGame.Interfaces
{
	public interface ILotteryService
	{
		void AddPlayer(Player player);
		public void PurchaseTickets(Player player, int ticketCount);
		void RunLottery();
		bool UserHasMoney(Player user);

	}
}
