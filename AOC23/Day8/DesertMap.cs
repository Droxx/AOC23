using System.Collections;
using System.Text.RegularExpressions;

namespace AOC23.Day8;

public class DesertMap
{
    private List<Direction> _directions = new();
    //private Dictionary<int, Map> _maps = new();
    private Dictionary<Location, Map> _maps = new();

    private Location _start = new Location("AAA");
    private Location _end = new Location("ZZZ");

    public long Navigate(string input)
    {
        ParseInput(input);

        var nodes = _maps.Where(m => m.Key.C == 'A').Select(c => new Location($"{c.Key.A}{c.Key.B}{c.Key.C}")).ToList();
        
        var currentLoc = _start;
        var directionIndex = 0;
        long steps = 0;

        var result = _maps
            .Where(d => d.Key.C == 'A')
            .Aggregate(1L, (total, next) => LeastCommonMultiple(total, Move(next.Key, (x) => x.C != 'Z')));
        
        return result;
    }

    private long Move(Location location, Func<Location, bool> it)
    {
        var result = 0;

        while (it(location))
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

        public override bool Equals(object? obj)
        {
            return ((Location)obj).A == A && ((Location)obj).B == B && ((Location)obj).C == C;
        }
        
        public override int GetHashCode() {
            return $"{A}{B}{C}".GetHashCode();
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