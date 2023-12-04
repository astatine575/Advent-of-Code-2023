using System.Text.RegularExpressions;

namespace Advent_2023.Day_3
{
    static class Solution
    {
        static bool IsSymbol (char c)
        {
            return c != '.'
                && (c < '0' || c > '9');
        }
        static bool ContainsSymbol(string str)
        {
            foreach (char c in str)
            {
                if (IsSymbol(c))
                {
                    return true;
                }
            }

            return false;
        }
        static bool IsNextToSymbol(int LineNumber, IEnumerable<string> InputLines, int StartIndex, int EndIndex)
        {
            // Substring top and bottom lines, as well as check current line start and end index for symbols.

            // First, check current line before-and-after (cheapest check)
            string CurrentLine = InputLines.ElementAt( LineNumber );
            if ( StartIndex >= 0 && IsSymbol( CurrentLine[StartIndex] ) )
            {
                return true;
            }

            if ( EndIndex < CurrentLine.Length && IsSymbol( CurrentLine[EndIndex] ) )
            {
                return true;
            }

            // Next, substring above and below lines (if valid) and look for symbol there
            StartIndex = Math.Max(StartIndex, 0);
            EndIndex = Math.Min(EndIndex, CurrentLine.Length - 1);
            if ( LineNumber > 0 )
            {
                string AboveLine = InputLines.ElementAt(LineNumber - 1);
                if (ContainsSymbol(AboveLine.Substring(StartIndex, EndIndex - StartIndex + 1)))
                {
                    return true;
                }
            }

            if (LineNumber < InputLines.Count() - 1)
            {
                string BelowLine = InputLines.ElementAt(LineNumber + 1);
                if (ContainsSymbol(BelowLine.Substring(StartIndex, EndIndex - StartIndex + 1)))
                {
                    return true;
                }
            }

            return false;
        }

        static void GetAdjacentNumbersInLine(string InputLine, int GearIndex, ref List<int> GearNumbers)
        {
            Regex NumberExp = new Regex(@"\d+");
            MatchCollection NumberMatches = NumberExp.Matches(InputLine);
            foreach (Match NumberMatch in NumberMatches)
            {
                int MatchStartIndex = NumberMatch.Index - 1;
                int MatchEndIndex = NumberMatch.Index + NumberMatch.Length;
                if (GearIndex >= MatchStartIndex && GearIndex <= MatchEndIndex)
                {
                    GearNumbers.Add(int.Parse(NumberMatch.Value));
                }
            }
        }

        static int GetGearRatio(int LineNumber, IEnumerable<string> InputLines, int GearIndex)
        {
            List<int> GearNumbers = new List<int>();
            // First, get the gears in the same line
            GetAdjacentNumbersInLine(InputLines.ElementAt(LineNumber), GearIndex, ref GearNumbers);

            // Next, get the numbers above and below our line
            if (LineNumber > 0)
            {
                string AboveLine = InputLines.ElementAt(LineNumber - 1);
                GetAdjacentNumbersInLine(AboveLine, GearIndex, ref GearNumbers);
            }

            if (LineNumber < InputLines.Count() - 1)
            {
                string BelowLine = InputLines.ElementAt(LineNumber + 1);
                GetAdjacentNumbersInLine(BelowLine, GearIndex, ref GearNumbers);
            }

            return GearNumbers.Count == 2 ? GearNumbers[0] * GearNumbers[1] : 0;
        }

        public static void Solve()
        {
            var InputLines = File.ReadLines(@"Day 3\Input.txt");
            int PartNumberSum = 0;
            for (int LineNumber = 0; LineNumber < InputLines.Count(); LineNumber++)
            {
                Regex NumberExp = new Regex(@"\d+");
                MatchCollection NumberMatches = NumberExp.Matches(InputLines.ElementAt(LineNumber));
                foreach (Match NumberMatch in NumberMatches)
                {
                    bool IsPart = IsNextToSymbol(LineNumber, InputLines, NumberMatch.Index - 1, NumberMatch.Index + NumberMatch.Length);
                    if (IsPart)
                    {
                        PartNumberSum += int.Parse(NumberMatch.Value);
                    }
                }
            }
            Console.WriteLine($"Part number sum is {PartNumberSum}");

            int GearRatioSum = 0;
            for (int LineNumber = 0; LineNumber < InputLines.Count(); LineNumber++)
            {
                Regex GearExp = new Regex(@"\*");
                MatchCollection GearMatches = GearExp.Matches(InputLines.ElementAt(LineNumber));
                foreach (Match GearMatch in GearMatches)
                {
                    GearRatioSum += GetGearRatio(LineNumber, InputLines, GearMatch.Index);
                }
            }
            Console.WriteLine($"Gear ratio sum is {GearRatioSum}");
        }
    }
}
