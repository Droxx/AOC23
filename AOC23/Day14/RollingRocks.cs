using System.Text;

namespace AOC23.Day14;

public class RollingRocks
{
    private readonly Grid _grid = new Grid();
    private const int CYCLES = 1000000000;

    //private Dictionary<string, int> KeysOfMultipleOfCycles = new Dictionary<string, int>();

    private Dictionary<string, List<int>> RepeatCounts = new Dictionary<string, List<int>>();
    public long Calculate(string input)
    {
        ParseInput(input);
        for(int i =0;i < CYCLES; i++)
        {
            
            var newKey = _grid.GetMatrixKey();
            if (RepeatCounts.TryGetValue(newKey, out var repeats))
            {
                repeats.Add(i);
                if (repeats.Count > 2)
                {
                    var diff = repeats[1] - repeats[0];
                    bool allSame = true;
                    for (var n = 2; n < repeats.Count; n++)
                    {
                        if(repeats[n] - repeats[n-1] != diff)
                        {
                            allSame = false;
                        }
                    }
                    
                    if(allSame)
                    {
                        var backTrackFrom1Bil = CYCLES % diff;
                        //i = CYCLES - backTrackFrom1Bil;
                        //continue;
                    }
                }
            }
            else
            {
                RepeatCounts.Add(newKey, new List<int> {i});
            }
            
            
            /*var newKey = _grid.GetMatrixKey();

            if (RepeatCounts.TryGetValue(newKey, out var lastSeen))
            {
                lastSeen.Add(i+1);
                if (CYCLES % (i + 1) == 0)
                {
                    var diff = lastSeen[1] - lastSeen[0];
                    bool allSame = true;
                    for (var n = 2; n < lastSeen.Count; n++)
                    {
                        if(lastSeen[n-1] - lastSeen[n] != diff)
                        {
                            allSame = false;
                        }
                    }
                    
                    if(allSame)
                    {
                        break;
                    }
                }
               
            }
            else
            {
                RepeatCounts.Add(newKey, new List<int> {i+1});
            }*/

            if (SumMatrix() == 64 && (i+1) % 7 != 0)
            {
                Console.WriteLine("Iteration {0} results in 64", i);
            }
            /*
            if (RepeatCounts.TryGetValue(newKey, out var count))
            {
                if (CYCLES % (i + 1) == 0 && count > 1)
                {
                    if (SumMatrix() == 64)
                    {
                        break;
                    }
                }
                RepeatCounts[newKey]++;
            }
            else
            {
                RepeatCounts.Add(newKey, 1);
            }*/

            CycleMatrix();
            
            //Console.Write("\r{0:N2}%", (i / (double)CYCLES) * 100);
        }
        Console.WriteLine();
        Console.WriteLine();
        
        _grid.PrintMatrix();
        long result = SumMatrix();
        return result;
    }

    private void CycleMatrix()
    {
        RollRocks();
        _grid.RotateMatrixClockwise();
        RollRocks();
        _grid.RotateMatrixClockwise();
        RollRocks();
        _grid.RotateMatrixClockwise();
        RollRocks();
        _grid.RotateMatrixClockwise();
    }

    private void Cycle()
    {
        // North
        _grid.TransposeRows();
        _grid.PrintGridRows();
        RollRocksNorth();
        _grid.PrintGridRows();
        
        // West
        _grid.TransposeRows();
        _grid.PrintGridRows();
        RollRocksNorth();
        _grid.PrintGridRows();
        
        // South
        _grid.TransposeRows();
        _grid.PrintGridRows();
        RollRocksNorth();
        _grid.PrintGridRows();
        
        // East
        _grid.TransposeRows();
        _grid.PrintGridRows();
        RollRocksNorth();
        _grid.PrintGridRows();
    }

    private long SumLoad()
    {
        _grid.PrintGridRows();;
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
        public List<string> Rows { get; set; }
        public char[,] Matrix { get; set; }

        // Rotate the Matrix 90 degrees clockwise
        public void RotateMatrixClockwise()
        {
            // Rotate Matrix 90 degrees clockwise
            var newMatrix = new char[Matrix.GetLength(1), Matrix.GetLength(0)];
            for (var x = 0; x < Matrix.GetLength(0); x++)
            {
                for (var y = 0; y < Matrix.GetLength(1); y++)
                {
                    newMatrix[y, Matrix.GetLength(0) - x - 1] = Matrix[x, y];
                }
            }

            Matrix = newMatrix;
        }

        public string GetMatrixKey()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < Matrix.GetLength(0); y++)
            {
                for (int x = 0; x < Matrix.GetLength(1); x++)
                {
                    sb.Append(Matrix[y, x]);
                }
            }

            return sb.ToString();
        }

        public void PrintMatrix()
        {
            for(int y = 0; y < Matrix.GetLength(0); y++)
            {
                for(int x = 0; x < Matrix.GetLength(1); x++)
                {
                    Console.Write(Matrix[y,x]);
                }
                Console.WriteLine();
            }
        }
        
        public void PrintGridRows()
        {
            foreach(var row in Rows)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine();
        }
        
        public void TransposeRows()
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