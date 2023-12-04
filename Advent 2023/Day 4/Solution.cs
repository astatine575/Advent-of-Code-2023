
namespace Advent_2023.Day_4
{
    static class Solution
    {
        public static void Solve()
        {
            var InputLines = File.ReadLines(@"Day 4\Input.txt");
            List<int> CardValues = new List<int>(InputLines.Count());
            foreach ( var InputLine in InputLines )
            {
                var CardDetails = InputLine.Split(':')[1].Split('|');

                HashSet<int> WinningNumbers = new HashSet<int>();
                foreach ( var WinningNumberStr in CardDetails[0].Split(' ') )
                {
                    int WinningNumber;
                    if ( int.TryParse(WinningNumberStr, out WinningNumber) )
                    {
                        WinningNumbers.Add(WinningNumber);
                    }
                }

                int NumWinningNumbers = 0;
                foreach (var NumberOnCardStr in CardDetails[1].Split(' '))
                {
                    int NumberOnCard;
                    if (int.TryParse(NumberOnCardStr, out NumberOnCard))
                    {
                        if (WinningNumbers.Contains(NumberOnCard))
                        {
                            NumWinningNumbers++;
                        }
                    }
                }

                CardValues.Add(NumWinningNumbers);
            }

            int TotalScore = 0;
            CardValues.ForEach(CardValue => TotalScore += CardValue > 0 ? 1 << (CardValue - 1) : 0);
            Console.WriteLine($"Total score is {TotalScore}");

            List<int> CardAmounts = new List<int>(CardValues.Count);
            CardValues.ForEach(CardValue => CardAmounts.Add(1));

            for ( int CardIndex = 0; CardIndex < CardValues.Count; CardIndex++ )
            {
                int NumCardsOfIndex = CardAmounts[CardIndex];
                int NumMatchingNumbers = CardValues[CardIndex];

                for (int AddingCardIndex = CardIndex + 1; AddingCardIndex < CardIndex + 1 + NumMatchingNumbers; AddingCardIndex++)
                {
                    CardAmounts[AddingCardIndex] += NumCardsOfIndex;
                }
            }

            int TotalNumCards = 0;
            CardAmounts.ForEach(CardAmount => TotalNumCards += CardAmount);
            Console.WriteLine($"Total number of cards is {TotalNumCards}");
        }
    }
}
