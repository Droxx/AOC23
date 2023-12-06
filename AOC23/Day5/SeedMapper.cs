using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AOC23.Day5;

public class SeedMapper
{
    private List<long> _seeds = new();

    private Map _seedToSoil = new();
    private Map _soilToFertilizer = new();
    private Map _fertilizerToWater = new();
    private Map _waterToLight = new();
    private Map _lightToTemp = new();
    private Map _tempToHumidity = new();
    private Map _humidityToLocation = new();
    
    
    public long GetClosestSeed(string input)
    {
        var sw = new Stopwatch();
        sw.Start();
        var lines = input.Split("\n");
        var seedPairs = GetSeedPairs(lines.First());
        //GetSeeds(lines.First());
        
        GetMapping("seed-to-soil map:", lines, _seedToSoil);
        GetMapping("soil-to-fertilizer map:", lines, _soilToFertilizer);
        GetMapping("fertilizer-to-water map:", lines, _fertilizerToWater);
        GetMapping("water-to-light map:", lines, _waterToLight);
        GetMapping("light-to-temperature map:", lines, _lightToTemp);
        GetMapping("temperature-to-humidity map:", lines, _tempToHumidity);
        GetMapping("humidity-to-location map:", lines, _humidityToLocation);

        long? currentClosestSeed = null;
        int pairCount = 1;
        foreach (var pair in seedPairs)
        {
            Console.WriteLine($"Working on seed pair ${pairCount} of 10");
            Console.WriteLine("Calculating seeds...");
            var seeds = GetSeedsFromPair(pair);
            var total = seeds.Count;
            var count = 0;
            Console.WriteLine($"Got {total} seeds, calculating...");
            foreach(long seed in seeds)
            {
                var soil = _seedToSoil.GetDest(seed);
                var fertilizer = _soilToFertilizer.GetDest(soil);
                var water = _fertilizerToWater.GetDest(fertilizer);
                var light = _waterToLight.GetDest(water);
                var temp = _lightToTemp.GetDest(light);
                var humidity = _tempToHumidity.GetDest(temp);
                var location = _humidityToLocation.GetDest(humidity);

                if (currentClosestSeed == null || location < currentClosestSeed)
                {
                    currentClosestSeed = location;
                }

                count++;
                if (count % 100000 == 0)
                {
                    Console.Write($"\r{(((decimal)count/(decimal)total) * 100).ToString("F1")}% complete");
                }
            }
            Console.Write("\n");
            Console.WriteLine($"Completed seed pair {pairCount}, took {sw.Elapsed.Minutes} minutes");
            pairCount++;
        }

        sw.Stop();
        Console.WriteLine($"Completed all seed pairs, took {sw.Elapsed.Minutes} minutes");
        return currentClosestSeed ?? 0;
    }

    private List<Tuple<long, long>> GetSeedPairs(string input)
    {
        var regex = new Regex(@"seeds:\s([\d+ ]+)");
        var matches = regex.Matches(input);
        var seedList = matches[0].Groups[1].Value.Split(" ").Where(n => long.TryParse(n, out var p)).Select(n => long.Parse(n)).ToList();
        var seedPairs = new List<Tuple<long, long>>();
        for(int i = 0; i < seedList.Count; i++)
        {
            seedPairs.Add(new Tuple<long,long>(seedList[i], seedList[i + 1]));

            i++;
        }

        return seedPairs;
    }

    private List<long> GetSeedsFromPair(Tuple<long, long> input)
    {
        var list = new List<long>();
        for(int c = 0; c < input.Item2; c++)
        {
            list.Add(input.Item1 + c);
        }

        return list;
    }
    
    private void GetSeeds(string input)
    {
        var regex = new Regex(@"seeds:\s([\d+ ]+)");
        var matches = regex.Matches(input);
        var seedList = matches[0].Groups[1].Value.Split(" ").Where(n => long.TryParse(n, out var p)).Select(n => long.Parse(n)).ToList();
        for(int i = 0; i < seedList.Count; i++)
        {
            var start = seedList[i];
            var count = seedList[i + 1];
            
            for(int c = 0; c < count; c++)
            {
                _seeds.Add(start + c);
            }

            i++;
        }
    }
    
    private void GetSeedsLegacy(string input)
    {
        var regex = new Regex(@"seeds:\s([\d+ ]+)");
        var matches = regex.Matches(input);
        _seeds = matches[0].Groups[1].Value.Split(" ").Where(n => long.TryParse(n, out var p)).Select(n => long.Parse(n)).ToList();
    }

    private void GetMapping(string title, string[] lines, Map mapping)
    {
        var currentLine = 1;
        while (lines[currentLine] != title)
        {
            currentLine++;
        }

        currentLine++;
        while (lines[currentLine] != "")
        {
            var parts = lines[currentLine].Split(" ");
            var part1 = long.Parse(parts[0]);
            var part2 = long.Parse(parts[1]);
            var part3 = long.Parse(parts[2]);
            mapping.AddMapping(part1, part2, part3);
            currentLine++;
        }
    }
}