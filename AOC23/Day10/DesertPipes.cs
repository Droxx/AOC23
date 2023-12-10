namespace AOC23.Day10;

public class DesertPipes
{
    private List<Tile> _map = new();

    private List<Tile> _visitedTiles = new List<Tile>();
    
    public long Navigate(string input)
    {
        ParseInput(input);
        var lines = input.Split('\n').Where(l => !string.IsNullOrEmpty(l));

        var startPosition = _map.First(m => m.Type == Tile.TileType.Start);

        var startingPipes = GetStartingPipes(startPosition);
        var steps = 1;
        
        var currentA = startingPipes.Item1;
        var previousA = startPosition;
        var currentB = startingPipes.Item2;
        var previousB = startPosition;
        Console.Clear();

        do
        {
            steps++;
            var newA = FollowPipe(currentA, previousA);
            var newB = FollowPipe(currentB, previousB);
            previousA = currentA;
            previousB = currentB;
            currentA = newA;
            currentB = newB;
            
            /*Console.WriteLine($"Step {steps}");
            for (int y = startPosition.Y - 10; y < startPosition.Y + 10; y++)
            {
                for (int x = startPosition.X - 10; x < startPosition.X + 10; x++)
                {
                    if (currentA.X == x && currentA.Y == y)
                    {
                        Console.Write('X');
                    }
                    else if (currentB.X == x && currentB.Y == y)
                    {
                        Console.Write('X');
                    }
                    else
                    {
                        Console.Write(lines.ElementAt(y)[x]);
                    }
                }
                Console.Write("\n\r");

            }
            Console.Write("\n\r");
            Console.Write("\n\r");*/
        } while (currentA != currentB && (currentA != startPosition || currentB != startPosition));


        return steps;
    }

    private Tile FollowPipe(Tile pipe, Tile previous)
    {
        var xDif = pipe.X - previous.X;
        var yDif = pipe.Y - previous.Y;
        switch (pipe.Direction)
        {
            case Tile.PipeDirection.Vertical:
                return _map.FirstOrDefault(t => t.X == previous.X && t.Y == previous.Y + (yDif*2));
            case Tile.PipeDirection.Horizontal:
                return _map.FirstOrDefault(t => t.X == previous.X + (xDif*2) && t.Y == previous.Y);
            case Tile.PipeDirection.NEBend:
                if (yDif == 1) // Pipe is below previous tile
                {
                    return _map.FirstOrDefault(t => t.X == previous.X + 1 && t.Y == previous.Y + 1);
                }

                return _map.FirstOrDefault(t => t.X == previous.X - 1  && t.Y == previous.Y - 1);
            case Tile.PipeDirection.NWBend:
                if (yDif == 1) // Pipe is below previous tile
                {
                    return _map.FirstOrDefault(t => t.X == previous.X - 1 && t.Y == previous.Y + 1);
                }
                return _map.FirstOrDefault(t => t.X == previous.X + 1 && t.Y == previous.Y - 1);  

            case Tile.PipeDirection.SEBend:
                if (yDif == -1) // Pipe is above previous tile
                {
                    return _map.FirstOrDefault(t => t.X == previous.X + 1 && t.Y == previous.Y - 1);
                }
                return _map.FirstOrDefault(t => t.X == previous.X - 1 && t.Y == previous.Y + 1);
            case Tile.PipeDirection.SWBend:
                if (yDif == -1) // Pipe is above previous tile
                {
                    return _map.FirstOrDefault(t => t.X == previous.X - 1  && t.Y == previous.Y - 1);
                }
                return _map.FirstOrDefault(t => t.X == previous.X + 1  && t.Y == previous.Y + 1);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private Tuple<Tile, Tile> GetStartingPipes(Tile startPosition)
    {
        Tile? first = null;
        for (var x = -1; x <= 1; x += 2)
        {
            var tile = _map.FirstOrDefault(t => t.X == startPosition.X + x && t.Y == startPosition.Y);
            if (tile != null && tile.Type == Tile.TileType.Pipe)
            {
                if (first == null)
                {
                    first = tile;
                }
                else
                {
                    return new Tuple<Tile, Tile>(first, tile);
                }
            }
        }

        for (var y = -1; y <= 1; y += 2)
        {
            var tile = _map.FirstOrDefault(t => t.X == startPosition.X && t.Y == startPosition.Y + y);
            if (tile != null && tile.Type == Tile.TileType.Pipe)
            {
                if (first == null)
                {
                    first = tile;
                }
                else
                {
                    return new Tuple<Tile, Tile>(first, tile);
                }
            }
        }

        throw new Exception("No starting pipes found");
    }

    private void ParseInput(string input)
    {
        var lines = input.Split('\n').Where(l => !string.IsNullOrEmpty(l));
        
        for (var y = 0; y < lines.Count(); y++)
        {
            var line = lines.ElementAt(y);
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                var tile = new Tile();
                if (c == 'S' || c == '.')
                {
                    tile = new Tile
                    {
                        X = x,
                        Y = y,
                        Type = (Tile.TileType)c
                    };

                }
                else
                {
                    tile = new Tile()
                    {
                        X = x,
                        Y = y,
                        Type = Tile.TileType.Pipe,
                        Direction = (Tile.PipeDirection)c
                    };
                }
                
                _map.Add(tile);
            }
        }
    }

    private class Tile
    {
        public TileType Type { get; set; }
        
        public int X { get; set; }
        public int Y { get; set; }
        
        public PipeDirection? Direction { get; set; }

        
        public enum TileType
        {
            Ground = '.',
            Start = 'S',
            Pipe = 'P'
        }
        
        public enum PipeDirection
        {
            Vertical = '|',
            Horizontal = '-',
            NEBend = 'L',
            NWBend = 'J',
            SEBend = 'F',
            SWBend = '7'
        }

        public override int GetHashCode()
        {
            return $"{X}{Y}".GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return ((Tile)this).GetHashCode() == ((Tile)obj).GetHashCode();
        }
    }
}