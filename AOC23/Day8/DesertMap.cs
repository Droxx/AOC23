using System.Collections;
using System.Text.RegularExpressions;

namespace AOC23.Day8;

public class DesertMap
{
    private List<Direction> _directions = new();
    private Dictionary<Location, Map> _maps = new();

    public long Navigate(string input)
    {
        ParseInput(input);
        
        var result = _maps
            .Where(d => d.Key.C == 'A')
            .Aggregate(1L, (total, node) => LeastCommonMultiple(total, GetStepsForNode(node.Key)));
        
        return result;
    }

    private long GetStepsForNode(Location location)
    {
        var result = 0;

        while (location.C != 'Z')
        {
            var direction = _directions[result % _directions.Count];
            location = direction == Direction.L ? _maps[location].Left : _maps[location].Right;
            result++;
        }

        return result++;
    }
    
    private void ParseInput(string input)
    {
        var lines = input.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        var dirLine = lines[0];
        
        foreach (var c in dirLine)
        {
            _directions.Add((Direction)c);
        }

        var regex = new Regex(@"([A-Z]+)");
        
        for(int i=1; i < lines.Count; i++)
        {
            var line = lines[i];
            var matches = regex.Matches(line);
            var loc = matches[0].Groups[1].Value;
            var left = matches[1].Groups[1].Value;
            var right = matches[2].Groups[1].Value;
            
            var map = new Map
            {
                Left = new Location(left),
                Right = new Location(right)
            };
            
            //_maps.Add(loc.Sum(c => c), map);
            _maps.Add(new Location(loc), map);
        }

    }

    private class Map
    {
        public Location Left { get; set; }
        public Location Right { get; set; }
    }

    private class Location
    {
        public char A { get; set; }
        public char B { get; set; }
        public char C { get; set; }

        public Location(string loc)
        {
            A = loc[0];
            B = loc[1];
            C = loc[2];
        }

        public override string ToString()
        {
            return $"{A}{B}{C}";
        }

        public override bool Equals(object? obj)
        {
            return ((Location)obj).A == A && ((Location)obj).B == B && ((Location)obj).C == C;
        }
        
        public override int GetHashCode() {
            return ToString().GetHashCode();
        }
    }
    
    private enum Direction
    {
        L = 'L',
        R = 'R'
    }

    private long GreatestCommonFactor(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
    
    private long LeastCommonMultiple(long a, long b)
    {
        return (a / GreatestCommonFactor(a, b)) * b;
    }
}