namespace AOC23.Day17;

public class HeatLoss
{
    private CityBlock[,] _grid;

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
            }
        }
    }


    private class CityBlock
    {
        public int HeatLoss { get; set; }
    }
}