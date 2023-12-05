
using System.Diagnostics;

namespace Advent_2023.Day_5
{
    static class Solution
    {
        struct AlmanacNode
        {
            public AlmanacNode(long Destination, long Source, long Length)
            {
                this.Destination = Destination;
                this.Source = Source;
                this.Length = Length;

            }

            public long Source { get; }
            public long Destination { get; }
            public long Length { get; }
        }

        static void ReadMap(int LineCount, IEnumerable<string> InputLines, out List<AlmanacNode> Map)
        {
            List<AlmanacNode> DataMap = new List<AlmanacNode>();
            while (++LineCount < InputLines.Count())
            {
                string InputLine = InputLines.ElementAt(LineCount);
                if (InputLine.Count() == 0)
                {
                    break;
                }

                string[] MapEntryStrings = InputLine.Split(' ');
                AlmanacNode MapEntry = new AlmanacNode(
                    long.Parse(MapEntryStrings[0]),
                    long.Parse(MapEntryStrings[1]),
                    long.Parse(MapEntryStrings[2])
                    );
                DataMap.Add(MapEntry);
            }
            DataMap.Sort((Node1, Node2) => Node1.Source > Node2.Source ? 1 : -1);

            Map = new List<AlmanacNode>(DataMap.Count*2);
            long LastNodeEnd = 0;
            foreach (AlmanacNode DataNode in DataMap)
            {
                // if there's a gap, add a filler
                if (LastNodeEnd != DataNode.Source)
                {
                    AlmanacNode FillerNode = new AlmanacNode(LastNodeEnd, LastNodeEnd, DataNode.Source - LastNodeEnd);
                    Map.Add(FillerNode);
                }

                Map.Add(DataNode);
                LastNodeEnd = DataNode.Source + DataNode.Length;
            }

            if (LastNodeEnd < long.MaxValue)
            {
                AlmanacNode EndNode = new AlmanacNode(LastNodeEnd, LastNodeEnd, long.MaxValue - LastNodeEnd);
                Map.Add(EndNode);
            }
        }

        static long LookupInMap(long Source, List<AlmanacNode> Map, ref long MinDistanceToNodeBoundary)
        {
            // Find what node might cover us in the map
            for (int i = 0; i < Map.Count; i++)
            {
                AlmanacNode Node = Map[i];
                if (Node.Source + Node.Length <= Source)
                {
                    continue;
                }

                long DistanceFromSource = Source - Node.Source;
                if (DistanceFromSource < MinDistanceToNodeBoundary)
                {
                    MinDistanceToNodeBoundary = DistanceFromSource;
                }

                return Node.Destination + DistanceFromSource;
            }

            throw new InvalidDataException("No node found for source!");
        }

        public static void Solve()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var InputLines = File.ReadLines(@"Day 5\Input.txt");

            // Read all the input maps

            string SeedLine = InputLines.ElementAt(0);
            string[] SeedStrings = SeedLine.Substring(7).Split(' ');
            List<Tuple<long, long>> InitialSeeds = new List<Tuple<long, long>>(SeedStrings.Count()/2);
            for (int i = 0; i < SeedStrings.Length - 1; i+=2)
            {
                long SeedRangeStart = long.Parse(SeedStrings[i]);
                long SeedRangeLength = long.Parse(SeedStrings[i+1]);
                long SeedRangeEnd = SeedRangeStart + SeedRangeLength;
                InitialSeeds.Add(new Tuple<long, long>(SeedRangeStart, SeedRangeEnd));
            }
            InitialSeeds.Sort((Tuple1, Tuple2) => Tuple2.Item1 > Tuple1.Item1 ? 1 : -1);

            // Maps we'll populate:
            List<AlmanacNode> SeedToSoil = null;
            List<AlmanacNode> SoilToFertilizer = null;
            List<AlmanacNode> FertilizerToWater = null;
            List<AlmanacNode> WaterToLight = null;
            List<AlmanacNode> LightToTemperature = null;
            List<AlmanacNode> TemperatureToHumidity = null;
            List<AlmanacNode> HumidityToLocation = null;

            // Read file line-by-line
            int LineCount = 0;
            while (++LineCount < InputLines.Count())
            {
                string InputLine = InputLines.ElementAt(LineCount);
                if ( InputLine.Contains("seed-to-soil"))
                {
                    ReadMap(LineCount, InputLines, out SeedToSoil);
                }
                else if (InputLine.Contains("soil-to-fertilizer"))
                {
                    ReadMap(LineCount, InputLines, out SoilToFertilizer);
                }
                else if (InputLine.Contains("fertilizer-to-water"))
                {
                    ReadMap(LineCount, InputLines, out FertilizerToWater);
                }
                else if (InputLine.Contains("water-to-light"))
                {
                    ReadMap(LineCount, InputLines, out WaterToLight);
                }
                else if (InputLine.Contains("light-to-temperature"))
                {
                    ReadMap(LineCount, InputLines, out LightToTemperature);
                }
                else if (InputLine.Contains("temperature-to-humidity"))
                {
                    ReadMap(LineCount, InputLines, out TemperatureToHumidity);
                }
                else if (InputLine.Contains("humidity-to-location"))
                {
                    ReadMap(LineCount, InputLines, out HumidityToLocation);
                }
            }

            // ...

            sw.Stop();

            Console.WriteLine($"Input Processing took {sw.ElapsedMilliseconds} milliseconds.");

            if (    SeedToSoil == null
                 || SoilToFertilizer == null
                 || FertilizerToWater == null
                 || WaterToLight == null
                 || LightToTemperature == null
                 || TemperatureToHumidity == null
                 || HumidityToLocation == null)
            {
                throw new InvalidDataException("Missing a map!");
            }

            sw.Restart();
            // Now find the smallest seed
            long SmallestSeed = long.MaxValue;
            long SmallestSeedLocation = long.MaxValue;
            long SkippedSeedCalculations = 0;
            long TotalSeedCalculations = 0;
            foreach (Tuple<long,long> SeedRange in InitialSeeds)
            {
                long SeedRangeStart = SeedRange.Item2;
                long SeedRangeEnd = SeedRange.Item1;
                for (long Seed = SeedRangeStart; Seed > SeedRangeEnd; Seed--)
                {
                    TotalSeedCalculations++;
                    long MinDistanceToNextBoundary = Seed - SeedRangeEnd;
                    long SeedSoil = LookupInMap(Seed, SeedToSoil, ref MinDistanceToNextBoundary);
                    long SeedFertilizer = LookupInMap(SeedSoil, SoilToFertilizer, ref MinDistanceToNextBoundary);
                    long SeedWater = LookupInMap(SeedFertilizer, FertilizerToWater, ref MinDistanceToNextBoundary);
                    long SeedLight = LookupInMap(SeedWater, WaterToLight, ref MinDistanceToNextBoundary);
                    long SeedTemperature = LookupInMap(SeedLight, LightToTemperature, ref MinDistanceToNextBoundary);
                    long SeedHumidity = LookupInMap(SeedTemperature, TemperatureToHumidity, ref MinDistanceToNextBoundary);
                    long SeedLocation = LookupInMap(SeedHumidity, HumidityToLocation, ref MinDistanceToNextBoundary);

                    // We can skip ahead by MinDistanceToNextBoundary to shortcut things
                    if (MinDistanceToNextBoundary > 0)
                    {
                        SkippedSeedCalculations += MinDistanceToNextBoundary;
                        Seed -= MinDistanceToNextBoundary - 1;
                        continue;
                    }

                    if (SeedLocation < SmallestSeedLocation)
                    {
                        SmallestSeedLocation = SeedLocation;
                        SmallestSeed = Seed;
                    }
                }
            }

            sw.Stop();

            Console.WriteLine($"Smallest location was {SmallestSeedLocation}. Data processing took {sw.ElapsedMilliseconds} milliseconds. Computed in {TotalSeedCalculations} seed mappings ({SkippedSeedCalculations} skipped).");
        }
    }
}
