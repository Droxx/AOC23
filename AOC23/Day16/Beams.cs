namespace AOC23.Day16;

public class Beams
{
    // [Y,X]
    private Tile[,] _grid;
    
    public long Calculate(string input)
    {
        ParseInput(input);

        return 5;
    }

    public void ParseInput(string input)
    {
        var lines = input.Split("\n").Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        
        _grid = new Tile[lines.Count,lines[0].Length];

        for (var y = 0; y < lines.Count; y++)
        {
            var lineCharArr = lines[y].ToCharArray();
            for (var x = 0; x < lineCharArr.Length; x++)
            {
                _grid[y, x] = new Tile
                {
                    IsEnergised = false,
                    Type = (Tile.TileType)lineCharArr[x]
                };
            }
        }
    }

    private class Tile
    {
        public bool IsEnergised { get; set; }
        public TileType Type { get; set; }

        public enum TileType
        {
            Neutral = '.',
            HorizontalSplitter = '-',
            VertialSplitter = '|',
            MirrorA = '/',
            MirrorB = '\\',
        }
    }
}