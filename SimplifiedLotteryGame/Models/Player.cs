using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedLotteryGame.Models
{
	public class Player
	{
		public int Id { get; set; }
		public int Money { get; set; } = 10;
		public List<Ticket> Tickets { get; set; } = new List<Ticket>();

		public bool CanPurchase(int ticketCount)
		{
			return Money >= ticketCount;
		}

        public void PurchaseTickets(IEnumerable<Ticket> tickets)
        {
            var ticketList = tickets.ToList();
            Tickets.AddRange(ticketList);
            Money -= ticketList.Sum(t => t.Price);
        }


        public bool HasMoney => Money > 0;
	}
}
