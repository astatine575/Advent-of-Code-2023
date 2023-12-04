using System.Text.RegularExpressions;

namespace Advent_2023.Day_2
{
    static class Solution
    {
        struct Game
        {
            public Game(int Id)
            {
                this.Id = Id;
            }
            public int Id { get; set; }
            public int MaxRed { get; set; } = 0;
            public int MaxGreen { get; set; } = 0;
            public int MaxBlue { get; set; } = 0;

            public bool IsValid()
            {
                return MaxRed <= 12 && MaxGreen <= 13 && MaxBlue <= 14;
            }

            public int PowerSet()
            {
                return MaxRed * MaxGreen * MaxBlue;
            }
        }

        public static void Solve()
        {
            var InputLines = File.ReadLines(@"Day 2\Input.txt");
            List<Game> Games = new List<Game>();
            foreach ( var InputLine in InputLines )
            {
                var SplitInputLine = InputLine.Split(':');
                int GameId = int.Parse(SplitInputLine[0].Substring(5));
                Game LineGame = new Game(GameId);

                Regex RedAmountExp = new Regex(@"\d+(?=( red))");
                MatchCollection RedAmountMatches = RedAmountExp.Matches(InputLine);
                foreach( Match RedAmountMatch in RedAmountMatches)
                {
                    int RedAmount = int.Parse(RedAmountMatch.Value);
                    if (RedAmount > LineGame.MaxRed)
                    {
                        LineGame.MaxRed = RedAmount;
                    }
                }

                Regex GreenAmountExp = new Regex(@"\d+(?=( green))");
                MatchCollection GreenAmountMatches = GreenAmountExp.Matches(InputLine);
                foreach (Match GreenAmountMatch in GreenAmountMatches)
                {
                    int GreenAmount = int.Parse(GreenAmountMatch.Value);
                    if (GreenAmount > LineGame.MaxGreen)
                    {
                        LineGame.MaxGreen = GreenAmount;
                    }
                }

                Regex BlueAmountExp = new Regex(@"\d+(?=( blue))");
                MatchCollection BlueAmountMatchess = BlueAmountExp.Matches(InputLine);
                foreach (Match BlueAmountMatch in BlueAmountMatchess)
                {
                    int BlueAmount = int.Parse(BlueAmountMatch.Value);
                    if (BlueAmount > LineGame.MaxBlue)
                    {
                        LineGame.MaxBlue = BlueAmount;
                    }
                }

                Games.Add(LineGame);
            }

            int TotalScore = 0;
            Games.ForEach(Game => TotalScore += Game.PowerSet());
            Console.WriteLine($"Total score is {TotalScore}");
        }
    }
}
