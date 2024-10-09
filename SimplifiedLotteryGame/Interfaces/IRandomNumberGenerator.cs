using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedLotteryGame.Interfaces
{
    public interface IRandomNumberGenerator
    {
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }
}
