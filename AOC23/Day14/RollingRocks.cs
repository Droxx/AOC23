namespace AOC23.Day14;

public class RollingRocks
{
    private readonly Grid _grid = new Grid();
    
    public long Calculate(string input)
    {
        ParseInput(input);
        RollRocksNorth();
        long result = SumLoad();
        return result;
    }

    private long SumLoad()
    {
        
        long total = 0;
        foreach (var col in _grid.Cols)
        {
            Console.WriteLine(col);
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i] == 'O')
                {
                    total += col.Length -i;
                }
            }
        }

        return total;
    }
    
    private void RollRocksNorth()
    {
        for (int x = 0; x < _grid.Cols.Count; x++)
        {
            var rollingBoulders = 0;
            for (int i = _grid.Cols[x].Length -1; i >= 0 ; i--)
            {
                var col = _grid.Cols[x];

                // If rolling boulder, remove and add to boulder list
                if (col[i] == 'O')
                {
                    rollingBoulders++;
                    var lineArr = col.ToCharArray();
                    lineArr[i] = '.';
                    _grid.Cols[x] = new string(lineArr);
                }
                // If rock boulder, stack pending boulders behind it
                if(col[i] == '#')
                {
                    var lineArr = col.ToCharArray();
                    for(var n = i+rollingBoulders; n > i; n--)
                    {
                        lineArr[n] = 'O';
                    }
                    _grid.Cols[x] = new string(lineArr);
                    rollingBoulders = 0;
                }
                else if (i == 0) // Otherwise, check if we're at the end
                {
                    var lineArr = col.ToCharArray();
                    for(var n = 0; n < rollingBoulders; n++)
                    {
                        lineArr[n] = 'O';
                    }
                    _grid.Cols[x] = new string(lineArr);
                    continue;
                }
            }
        }
    }

    private void ParseInput(string input)
    {
        var lines = input.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        _grid.Rows = lines;

        var cols = new string[lines.Count];

        for (var x = 0; x < lines[0].Length; x++)
        {
            for (var y = 0; y < lines.Count; y++)
            {
                cols[x] += lines[y][x];
            }
        }

        _grid.Cols = cols.ToList();
    }

    private class Grid
    {
        public List<string> Rows { get; set; } = new List<string>();
        public List<string> Cols { get; set; } = new List<string>();
    }
}