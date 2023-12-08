using System.Text.RegularExpressions;

namespace AOC23.Day8;

public class DesertMap
{
    private List<Direction> _directions = new();
    private Dictionary<int, Map> _maps = new();

    private int _start = "AAA".Sum(c => c);
    private int _end = "ZZZ".Sum(c => c);
    
    public long Navigate(string input)
    {
        ParseInput(input);

        var currentLoc = _start;
        var directionIndex = 0;
        var steps = 0;
        
        while (currentLoc != _end)
        {
            var direction = _directions[directionIndex];

            switch (direction)
            {
                case Direction.L:
                    currentLoc = _maps[currentLoc].Left;
                    break;
                case Direction.R:
                    currentLoc = _maps[currentLoc].Right;
                    break;
            }
            
            if(directionIndex == _directions.Count - 1)
                directionIndex = 0;
            else
                directionIndex++;

            steps++;
        }

        return steps;
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
                Left = right.Sum(c => c),
                Right = left.Sum(c => c)
            };
            
            _maps.Add(loc.Sum(c => c), map);
        }

    }

    private class Map
    {
        public int Left { get; set; }
        public int Right { get; set; }
    }
    
    private enum Direction
    {
        L = 'L',
        R = 'R'
    }
}