using SimplifiedLotteryGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedLotteryGame.Helpers
{
    public class GenerateRandomNumber : IRandomNumberGenerator
    {
        private readonly Random _random = new Random();

        public int Next(int maxValue)
        {
            return RandomNumberGenerator.GetInt32(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return RandomNumberGenerator.GetInt32(minValue, maxValue);
        }
    }
}
