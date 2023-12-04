using System.Text.RegularExpressions;

namespace Advent_2023.Day_1
{
    static class Solution
    {
        static int GetDigit(string Digit)
        {
            if (Digit.Length == 1)
            {
                return Digit[0] - '0';
            }

            if (Digit == "one")
            {
                return 1;
            }
            if (Digit == "two")
            {
                return 2;
            }
            if (Digit == "three")
            {
                return 3;
            }
            if (Digit == "four")
            {
                return 4;
            }
            if (Digit == "five")
            {
                return 5;
            }
            if (Digit == "six")
            {
                return 6;
            }
            if (Digit == "seven")
            {
                return 7;
            }
            if (Digit == "eight")
            {
                return 8;
            }
            if (Digit == "nine")
            {
                return 9;
            }

            return -1;
        }

        public static void Solve()
        {
            var InputLines = File.ReadLines(@"Day 1\Input.txt");
            List<int> LineValues = new List<int>();
            foreach (var InputLine in InputLines)
            {
                Regex Digit = new Regex(@"\d|one|two|three|four|five|six|seven|eight|nine");
                Match DigitMatch = Digit.Match(InputLine);

                int FirstDigit = GetDigit(DigitMatch.Value);

                int LastDigit = FirstDigit;
                while (DigitMatch.Success)
                {
                    LastDigit = GetDigit(DigitMatch.Value);
                    DigitMatch = Digit.Match(InputLine, DigitMatch.Index + 1);
                }

                LineValues.Add(FirstDigit * 10 + LastDigit);
            }

            int TotalValue = 0;
            LineValues.ForEach(LineValue => TotalValue += LineValue);
            Console.WriteLine($"Total value is {TotalValue}");
        }
    }
}
