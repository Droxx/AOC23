namespace AOC23.Day11;

public class Galaxies
{
    private Dictionary<int, Galaxy> _galaxies = new();
    private int _yMax = 0;
    private int _xMax = 0;

    public long Calculate(string input)
    {
        Parse(input);
        Expand();

        CalculateDistances();

        return _galaxies.Sum(g => g.Value.Distances.Sum(d => d.Value));
    }

    private void CalculateDistances()
    {
        foreach (var startingPoint in _galaxies)
        {
            foreach (var testGalaxy in _galaxies.Where(g => g.Key != startingPoint.Key && !g.Value.Distances.ContainsKey(startingPoint.Key)))
            {
                var xDif = startingPoint.Value.X - testGalaxy.Value.X;
                var yDif = startingPoint.Value.Y - testGalaxy.Value.Y;
                
                var distance = Math.Abs(xDif) + Math.Abs(yDif);
                startingPoint.Value.Distances.Add(testGalaxy.Key, distance);
            }
        }
    }
    
    private void Expand()
    {
        var emptyYRows = new List<int>();
        var emptyXRows = new List<int>();
        
        for (var y = 0; y < _yMax; y++)
        {
            if(_galaxies.All(g => g.Value.Y != y))
            {
                // No galaxies in this row
                emptyYRows.Add(y);
            }
        }
        
        for (var x = 0; x < _xMax; x++)
        {
            if(_galaxies.All(g => g.Value.X != x))
            {
                // No galaxies in this row
                emptyXRows.Add(x);
            }
        }
        
        for(var y = _yMax - 1; y >= 0; y--)
        {
            if(emptyYRows.Contains(y))
            {
                // Shift galaxies after this row to the down (Y+1)
                foreach (var galaxy in _galaxies.Where(g => g.Value.Y > y))
                {
                    galaxy.Value.Y += 999999;
                }
            }
        }
        
        for(var x = _xMax - 1; x >= 0; x--)
        {
            if(emptyXRows.Contains(x))
            {
                // Shift galaxies after this row to the right (X+1)
                foreach (var galaxy in _galaxies.Where(g => g.Value.X > x))
                {
                    galaxy.Value.X += 999999;
                }
            }
        }
        

        _yMax += (emptyYRows.Count * 1000000);
        _xMax += (emptyXRows.Count * 1000000);
    }

    private void Parse(string input)
    {
        var lines = input.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        var galaxyCount = 0;

        _yMax = lines.Count;
        _xMax = lines[0].Length;
        
        for (int y = 0; y < lines.Count; y++)
        {
            for(int x= 0; x < lines[y].Length; x++)
            {
                var c = lines[y][x];
                if (c == '.')
                {
                    continue;
                }

                var galaxy = new Galaxy
                {
                    X = x,
                    Y = y,
                    //Distance = 0,
                    //ClosestGalaxy = null
                };
                
                _galaxies.Add(galaxyCount, galaxy);
                galaxyCount++;
            }
        }
    }

    // Parse
    // Expand (add empty rows and columns)
    // For each galaxy, keep iterating away from it until find galaxy
      // If found, make that distance max, keep iterating until find another, or hit max
      // Add to array
      // Move onto next galaxy
    // If find a galaxy that has a closer galaxy than it\s origingal pairing, then add that galaxy to a redo list
    // Keep repeating this task until redo list is empty
    
    private class Galaxy
    {
        public long X { get; set; }
        public long Y { get; set; }
        public Dictionary<int, long> Distances { get; set; } = new();
        /*public int Distance { get; set; }
        public int? ClosestGalaxy { get; set; }*/
    }
}