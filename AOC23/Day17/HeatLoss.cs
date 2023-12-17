namespace AOC23.Day17;

public class HeatLoss
{
    private CityBlock[,] _grid;
    private int[,] _graph;

    private CityBlock _start;
    private CityBlock _end;
    
    public long Calculate(string input)
    {
        ParseInput(input);
        return 4;
    }
    
    
    
    private void ParseInput(string input)
    {
        var lines = input.Split("\n").Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        _grid = new CityBlock[lines.Count,lines[0].Length];
        for (int y = 0; y < lines.Count; y++)
        {
            for(int x = 0; x < lines[0].Length; x++)
            {
                _grid[y, x] = new CityBlock
                {
                    HeatLoss = int.Parse(lines[y][x].ToString())
                };
                _graph[y, x] = int.Parse(lines[y][x].ToString());
            }
        }
        _start = _grid[0, 0];
        _end = _grid[lines.Count - 1, lines[0].Length - 1];
    }


    private static void Print(int[] distance, int verticesCount)
    {
        Console.WriteLine("Vertex    Distance from source");

        for (int i = 0; i < verticesCount; ++i)
            Console.WriteLine("{0}\t  {1}", i, distance[i]);
    }

    
    
    private class CityBlock
    {
        public int HeatLoss { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public override int GetHashCode()
        {
            return (Y * 1000) + X;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is CityBlock other)
            {
                return other.X == X && other.Y == Y;
            }

            return false;
        }
    }
}