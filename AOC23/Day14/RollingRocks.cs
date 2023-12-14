namespace AOC23.Day14;

public class RollingRocks
{
    private readonly Grid _grid = new Grid();
    private const int CYCLES = 1;
    public long Calculate(string input)
    {
        ParseInput(input);
        for(int i =0;i < CYCLES; i++)
        {
            //_grid.Transpose();
            //RollRocksNorth();
            RollRocks();
        }
        long result = SumMatrix();
        return result;
    }

    private void Cycle()
    {
        // North
        _grid.Transpose();
        _grid.PrintGrid();
        RollRocksNorth();
        _grid.PrintGrid();
        
        // West
        _grid.Transpose();
        _grid.PrintGrid();
        RollRocksNorth();
        _grid.PrintGrid();
        
        // South
        _grid.Transpose();
        _grid.PrintGrid();
        RollRocksNorth();
        _grid.PrintGrid();
        
        // East
        _grid.Transpose();
        _grid.PrintGrid();
        RollRocksNorth();
        _grid.PrintGrid();
    }

    private long SumLoad()
    {
        _grid.PrintGrid();;
        long total = 0;
        foreach (var col in _grid.Rows)
        {
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
    
    private long SumMatrix()
    {
        long total = 0;
        for(int y =0; y < _grid.Matrix.GetLength(0); y++)
        {
            for(int x =0; x < _grid.Matrix.GetLength(1); x++)
            {
                if(_grid.Matrix[y,x] == 'O')
                {
                    total += _grid.Matrix.GetLength(0) - y;
                }
            }
        }

        return total;
    }

    private void RollRocks()
    {
        for (int x = 0; x < _grid.Matrix.GetLength(0); x++)
        {
            var rollingBoulders = 0;
            for (int y = _grid.Matrix.GetLength(1) -1; y >= 0 ; y--)
            {
                // If rolling boulder, remove and add to boulder list
                if (_grid.Matrix[y,x] == 'O')
                {
                    rollingBoulders++;
                    _grid.Matrix[y,x] = '.';
                }
                // If rock boulder, stack pending boulders behind it
                if(_grid.Matrix[y,x] == '#')
                {
                    for(var n = y+rollingBoulders; n > y; n--)
                    {
                        _grid.Matrix[n, x] = 'O';
                    }
                    rollingBoulders = 0;
                }
                else if (y == 0) // Otherwise, check if we're at the end
                {
                    for(var n = 0; n < rollingBoulders; n++)
                    {
                        _grid.Matrix[n,x] = 'O';
                    }
                }
            }
        }
    }
    
    private void RollRocksNorth()
    {
        for (int x = 0; x < _grid.Rows.Count; x++)
        {
            var rollingBoulders = 0;
            for (int i = _grid.Rows[x].Length -1; i >= 0 ; i--)
            {
                var col = _grid.Rows[x];

                // If rolling boulder, remove and add to boulder list
                if (col[i] == 'O')
                {
                    rollingBoulders++;
                    var lineArr = col.ToCharArray();
                    lineArr[i] = '.';
                    _grid.Rows[x] = new string(lineArr);
                }
                // If rock boulder, stack pending boulders behind it
                if(col[i] == '#')
                {
                    var lineArr = col.ToCharArray();
                    for(var n = i+rollingBoulders; n > i; n--)
                    {
                        lineArr[n] = 'O';
                    }
                    _grid.Rows[x] = new string(lineArr);
                    rollingBoulders = 0;
                }
                else if (i == 0) // Otherwise, check if we're at the end
                {
                    var lineArr = col.ToCharArray();
                    for(var n = 0; n < rollingBoulders; n++)
                    {
                        lineArr[n] = 'O';
                    }
                    _grid.Rows[x] = new string(lineArr);
                    continue;
                }
            }
        }
    }

    private void ParseInput(string input)
    {
        var lines = input.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        _grid.Rows = lines;

        _grid.Matrix = new char[lines.Count, lines[0].Length];

        for (int y = 0; y < lines.Count; y++)
        {
            var line = lines[y].ToCharArray();
            for (var x = 0; x < lines[0].Length; x++)
            {
                _grid.Matrix[y, x] = line[x];
            }
        }
    }

    private class Grid
    {
        public char[,] Matrix { get; set; }
        public List<string> Rows { get; set; } = new List<string>();

        public void PrintGrid()
        {
            foreach(var row in Rows)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine();
        }
        
        public void Transpose()
        {
            var newList = new string[Rows.Count];
            for (var x = 0; x < Rows[0].Length; x++)
            {
                for (var y = 0; y < Rows.Count; y++)
                {
                    newList[x] += Rows[y][x];
                }
            }

            Rows = newList.ToList();
        }
    }
}