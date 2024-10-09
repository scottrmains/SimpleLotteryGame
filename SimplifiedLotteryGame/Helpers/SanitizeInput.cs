using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedLotteryGame.Helpers
{
    public class HelperMethods
    {

        public static int? SanitizeInput(string number)
        {
            try
            {
                var sanitizedNumber = Int32.Parse(number);
                if (sanitizedNumber < 1 || sanitizedNumber > 10)
                {
                    Console.WriteLine("You can only purchase between 1 and 10 tickets.");
                    return null;
                }
                return sanitizedNumber;
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return null;
            }
        }

    }
}
