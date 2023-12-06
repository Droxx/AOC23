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
        var lines = input.Split("\n");
        GetSeeds(lines.First());
        
        GetMapping("seed-to-soil map:", lines, _seedToSoil);
        GetMapping("soil-to-fertilizer map:", lines, _soilToFertilizer);
        GetMapping("fertilizer-to-water map:", lines, _fertilizerToWater);
        GetMapping("water-to-light map:", lines, _waterToLight);
        GetMapping("light-to-temperature map:", lines, _lightToTemp);
        GetMapping("temperature-to-humidity map:", lines, _tempToHumidity);
        GetMapping("humidity-to-location map:", lines, _humidityToLocation);
        
        return 0;
    }

    private void GetSeeds(string input)
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