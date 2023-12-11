namespace Day5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = GetStrings();
            var seeds = new List<SeedToLocation>();
            var seedToSoilMap = new List<MapMinMaxRange>();
            var soilToFertMap = new List<MapMinMaxRange>();
            var fertToWaterMap = new List<MapMinMaxRange>();
            var waterToLightMap = new List<MapMinMaxRange>();
            var lightToTempMap = new List<MapMinMaxRange>();
            var tempToHumidMap = new List<MapMinMaxRange>();
            var humidToLocationMap = new List<MapMinMaxRange>();

            // get all seeds from string
            var seedsString = lines[0].Replace("seeds: ", "").Split(" ");

            foreach (var seed in seedsString)
            {
                seeds.Add(GetNewSeedMap(double.Parse(seed)));
            }

            GetDataForMaps(lines, "seed", "soil", ref seedToSoilMap);
            GetDataForMaps(lines, "soil", "fert", ref soilToFertMap);
            GetDataForMaps(lines, "fert", "water", ref fertToWaterMap);
            GetDataForMaps(lines, "water", "light", ref waterToLightMap);
            GetDataForMaps(lines, "light", "temp", ref lightToTempMap);
            GetDataForMaps(lines, "temp", "humid", ref tempToHumidMap);
            GetDataForMaps(lines, "humid", "location", ref humidToLocationMap);

            for (var i = 0; i < seeds.Count; i++)
            {
                // work out seed to soil

                foreach (var item in seedToSoilMap)
                {
                    if (IsInRange(item.secondInt, item.rangeInt, seeds[i].seed))
                    {
                        var difference = GetDifference(item.secondInt, seeds[i].seed);

                        var test = seeds.ElementAt(i);

                        test.soil = item.firstInt + difference;

                        seeds[i] = test;

                        break;
                    }
                }

                if (seeds[i].soil == 0)
                {
                    var test = seeds.ElementAt(i);

                    test.soil = test.seed;

                    seeds[i] = test;
                }

                foreach (var item in soilToFertMap)
                {
                    if (IsInRange(item.secondInt, item.rangeInt, seeds[i].soil))
                    {
                        var difference = GetDifference(item.secondInt, seeds[i].soil);

                        var test = seeds.ElementAt(i);

                        test.fertilizer = item.firstInt + difference;

                        seeds[i] = test;

                        break;
                    }
                }

                if (seeds[i].fertilizer == 0)
                {
                    var test = seeds.ElementAt(i);

                    test.fertilizer = test.soil;

                    seeds[i] = test;
                }

                foreach (var item in fertToWaterMap)
                {
                    if (IsInRange(item.secondInt, item.rangeInt, seeds[i].fertilizer))
                    {
                        var difference = Math.Abs(GetDifference(item.secondInt, seeds[i].fertilizer));

                        var test = seeds.ElementAt(i);

                        test.water = item.firstInt + difference;

                        seeds[i] = test;

                        break;
                    }
                }

                if (seeds[i].water == 0)
                {
                    var test = seeds.ElementAt(i);

                    test.water = test.fertilizer;

                    seeds[i] = test;
                }

                foreach (var item in waterToLightMap)
                {
                    if (IsInRange(item.secondInt, item.rangeInt, seeds[i].water))
                    {
                        var difference = GetDifference(item.secondInt, seeds[i].water);

                        var test = seeds.ElementAt(i);

                        test.light = item.firstInt + difference;

                        seeds[i] = test;

                        break;
                    }
                }

                if (seeds[i].light == 0)
                {
                    var test = seeds.ElementAt(i);

                    test.light = test.water;

                    seeds[i] = test;
                }

                foreach (var item in lightToTempMap)
                {
                    if (IsInRange(item.secondInt, item.rangeInt, seeds[i].light))
                    {
                        var difference = GetDifference(item.secondInt, seeds[i].light);

                        var test = seeds.ElementAt(i);

                        test.temperature = item.firstInt + difference;

                        seeds[i] = test;

                        break;
                    }
                }

                if (seeds[i].temperature == 0)
                {
                    var test = seeds.ElementAt(i);

                    test.temperature = test.light;

                    seeds[i] = test;
                }

                foreach (var item in tempToHumidMap)
                {
                    if (IsInRange(item.secondInt, item.rangeInt, seeds[i].temperature))
                    {
                        var difference = GetDifference(item.secondInt, seeds[i].temperature);

                        var test = seeds.ElementAt(i);

                        test.humidity = item.firstInt + difference;

                        seeds[i] = test;

                        break;
                    }
                }

                if (seeds[i].humidity == 0)
                {
                    var test = seeds.ElementAt(i);

                    test.humidity = test.temperature;

                    seeds[i] = test;
                }

                foreach (var item in humidToLocationMap)
                {
                    if (IsInRange(item.secondInt, item.rangeInt, seeds[i].humidity))
                    {
                        var difference = GetDifference(item.secondInt, seeds[i].humidity);

                        var test = seeds.ElementAt(i);

                        test.location = item.firstInt + difference;

                        seeds[i] = test;

                        break;
                    }
                }

                if (seeds[i].location == 0)
                {
                    var test = seeds.ElementAt(i);

                    test.location = test.humidity;

                    seeds[i] = test;
                }                
            }

            var lowest = 0.0;

            foreach (var item in seeds)
            {
                if (lowest == 0 || item.location < lowest)
                {
                    lowest = item.location;
                }
            }

            Console.WriteLine(lowest.ToString());
        }

        static double GetDifference(double first, double second)
        {
            return Math.Abs(first - second);
        }

        static bool IsInRange(double bottom, double top, double number)
        {
            return number >= bottom && number <= (bottom + top);
        }

        static void GetDataForMaps(string[] lines, string first, string second, ref List<MapMinMaxRange> map)
        {
            var whileBreak = true;
            var mapText = "map:";

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(first) && lines[i].Contains(second) && lines[i].Contains(mapText))
                {
                    var index = i;

                    while (whileBreak)
                    {
                        index++;

                        if (index >= lines.Length || lines[index].Contains(mapText))
                        {
                            return;
                        }

                        var numbersSplit = lines[index].Split(" ");

                        map.Add(new MapMinMaxRange { firstInt = double.Parse(numbersSplit[0]), secondInt = double.Parse(numbersSplit[1]), rangeInt = double.Parse(numbersSplit[2]) });
                    }
                }
            }
        }

        static string[] GetStrings()
        {
            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\input\givenInput.txt"));

            var text = File.ReadAllText(path);

            return text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        static string[] GetTestData()
        {
            return new string[]
            {
                "seeds: 79 14 55 13",
                "seed-to-soil map:",
                "50 98 2",
                "52 50 48",
                "soil-to-fertilizer map:",
                "0 15 37",
                "37 52 2",
                "39 0 15",
                "fertilizer-to-water map:",
                "49 53 8",
                "0 11 42",
                "42 0 7",
                "57 7 4",
                "water-to-light map:",
                "88 18 7",
                "18 25 70",
                "light-to-temperature map:",
                "45 77 23",
                "81 45 19",
                "68 64 13",
                "temperature-to-humidity map:",
                "0 69 1",
                "1 0 69",
                "humidity-to-location map:",
                "60 56 37",
                "56 93 4",
            };
        }

        static MapMinMaxRange GetNewMap(double first, double second, double range)
        {
            return new MapMinMaxRange() { firstInt = first, secondInt = second, rangeInt = range };
        }

        static SeedToLocation GetNewSeedMap(double seed)
        {
            return new SeedToLocation() { seed = seed };
        }

        struct MapMinMaxRange
        {
            public double firstInt;
            public double secondInt;
            public double rangeInt;
        }

        struct SeedToLocation
        {
            public double seed;
            public double soil;
            public double fertilizer;
            public double water;
            public double light;
            public double temperature;
            public double humidity;
            public double location;
        }
    }
}

/*
 --- Day 5: If You Give A Seed A Fertilizer ---

You take the boat and find the gardener right where you were told he would be: managing a giant "garden" that looks more to you like a farm.

"A water source? Island Island is the water source!" You point out that Snow Island isn't receiving any water.

"Oh, we had to stop the water because we ran out of sand to filter it with! Can't make snow with dirty water. Don't worry, 
I'm sure we'll get more sand soon; we only turned off the water a few days... weeks... oh no." His face sinks into a look of horrified realization.

"I've been so busy making sure everyone here has food that I completely forgot to check why we stopped getting more sand! 
There's a ferry leaving soon that is headed over in that direction - it's much faster than your boat. Could you please go check it out?"

You barely have time to agree to this request when he brings up another. "While you wait for the ferry, maybe you can help us with our food production problem. 
The latest Island Island Almanac just arrived and we're having trouble making sense of it."

The almanac (your puzzle input) lists all of the seeds that need to be planted. It also lists what type of soil to use with each kind of seed, 
what type of fertilizer to use with each kind of soil, what type of water to use with each kind of fertilizer, and so on. Every type of seed, soil, 
fertilizer and so on is identified with a number, but numbers are reused by each category - that is, soil 123 and fertilizer 123 aren't necessarily related to each other.

For example:

seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4

The almanac starts by listing which seeds need to be planted: seeds 79, 14, 55, and 13.

The rest of the almanac contains a list of maps which describe how to convert numbers from a source category into numbers in a destination category. 
That is, the section that starts with seed-to-soil map: describes how to convert a seed number (the source) to a soil number (the destination). 
This lets the gardener and his team know which soil to use with which seeds, which water to use with which fertilizer, and so on.

Rather than list every source number and its corresponding destination number one by one, the maps describe entire ranges of numbers that can be converted. 
Each line within a map contains three numbers: the destination range start, the source range start, and the range length.

Consider again the example seed-to-soil map:

50 98 2
52 50 48

The first line has a destination range start of 50, a source range start of 98, and a range length of 2. This line means that the source range starts at 98 and contains two values: 98 and 99. 
The destination range is the same length, but it starts at 50, so its two values are 50 and 51. With this information, you know that seed number 98 corresponds to soil number 50 and that seed number 99 corresponds to soil number 51.

The second line means that the source range starts at 50 and contains 48 values: 50, 51, ..., 96, 97. 
This corresponds to a destination range starting at 52 and also containing 48 values: 52, 53, ..., 98, 99. So, seed number 53 corresponds to soil number 55.

Any source numbers that aren't mapped correspond to the same destination number. So, seed number 10 corresponds to soil number 10.

So, the entire list of seed numbers and their corresponding soil numbers looks like this:

seed  soil
0     0
1     1
...   ...
48    48
49    49
50    52
51    53
...   ...
96    98
97    99
98    50
99    51

With this map, you can look up the soil number required for each initial seed number:

    Seed number 79 corresponds to soil number 81.
    Seed number 14 corresponds to soil number 14.
    Seed number 55 corresponds to soil number 57.
    Seed number 13 corresponds to soil number 13.

The gardener and his team want to get started as soon as possible, so they'd like to know the closest location that needs a seed. Using these maps, 
find the lowest location number that corresponds to any of the initial seeds. To do this, you'll need to convert each seed number through other categories until you can find its corresponding location number. 
In this example, the corresponding types are:

    Seed 79, soil 81, fertilizer 81, water 81, light 74, temperature 78, humidity 78, location 82.
    Seed 14, soil 14, fertilizer 53, water 49, light 42, temperature 42, humidity 43, location 43.
    Seed 55, soil 57, fertilizer 57, water 53, light 46, temperature 82, humidity 82, location 86.
    Seed 13, soil 13, fertilizer 52, water 41, light 34, temperature 34, humidity 35, location 35.

So, the lowest location number in this example is 35.

What is the lowest location number that corresponds to any of the initial seed numbers?

*/