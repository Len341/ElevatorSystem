using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Utils
{
    public static class GeneralHelper
    {
        private static readonly object _lock = new();

        public static void WriteLine(string message)
        {
            lock (_lock)
            {
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public static string ToOrdinal(int number)
        {
            if (number <= 0) return number.ToString();

            int lastTwoDigits = number % 100;
            int lastDigit = number % 10;

            string suffix = "th"; // Default

            if (lastTwoDigits < 11 || lastTwoDigits > 13)
            {
                switch (lastDigit)
                {
                    case 1: suffix = "st"; break;
                    case 2: suffix = "nd"; break;
                    case 3: suffix = "rd"; break;
                }
            }

            return $"{number}{suffix}";
        }
    }
}
