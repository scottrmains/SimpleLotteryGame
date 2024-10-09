using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedLotteryGame.Models
{
	public class Ticket
	{
        private static int _currentTicketNumber = 0;
        public int Number { get; set; }
		public int Price { get; set; } = 1;

        public Ticket()
        {
            Number = GetNextTicketNumber();
        }

        private static int GetNextTicketNumber()
        {
            return Interlocked.Increment(ref _currentTicketNumber);
        }
        public static void ResetTicketNumber()
        {
            Interlocked.Exchange(ref _currentTicketNumber, 0);
        }

        public static int GetTicketNumber()
        {
            return _currentTicketNumber;
        }
    }
}
