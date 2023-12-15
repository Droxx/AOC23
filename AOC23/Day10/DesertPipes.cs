using System.Drawing;

namespace AOC23.Day10;

public class DesertPipes
{
    private List<Tile> _map = new();

    private List<Tile> _visitedTiles = new List<Tile>();
    private List<Tile> _encompassed = new List<Tile>();
    
    public long Navigate(string input)
    {
        ParseInput(input);
        var lines = input.Split('\n').Where(l => !string.IsNullOrEmpty(l));

        var startPosition = _map.First(m => m.Type == Tile.TileType.Start);

        var startingPipes = GetStartingPipes(startPosition);
        var steps = 1;
        
        var currentA = startingPipes.Item1;
        var previousA = startPosition;
        _visitedTiles.Add(currentA);
        var currentB = startingPipes.Item2;
        var previousB = startPosition;
        Console.Clear();

        do
        {
            steps++;
            var newA = FollowPipe(currentA, previousA);
            previousA = currentA;
            currentA = newA;
            
            
            
            
            _visitedTiles.Add(currentA);
            
            Console.Write($"On step {steps} of ~14000\r");
            
        } while (currentA != startPosition);

        bool inside = false;
        int areaCount = 0;

        for (var y = 0; y <= _map.Max(m => m.Y); y++)
        {
            for (var x = 0; x <= _map.Max(m => m.X); x++)
            {
                // We can scan through each tile, and determine when we are inside, or outside the shape. When we are inside and we encounter a non-visited tile
                // Then we can increase an area count

                var currentTile = _map.First(m => m.X == x && m.Y == y);
                if (_visitedTiles.Contains(currentTile))
                {
                    // Change inside bool depending on rules
                }
                else
                {
                    if (inside)
                    {
                        areaCount++;
                    }
                }
            }
        }

        Console.Write("\n\r");
        Console.Write("\n\r");

        
        return 1;
    }

    private bool AnyAdjacentVisitedBefore(Tile current, Tile previous, out Tile? adjacent)
    {
        for (var x = -1; x <= 1; x += 2)
        {
            var tile = _visitedTiles.FirstOrDefault(t => t.X == current.X + x && t.Y == current.Y);
            if (tile != null && !tile.Equals(previous))
            {
                adjacent = tile;
                return true;
            }
        }

        for (var y = -1; y <= 1; y += 2)
        {
            var tile = _visitedTiles.FirstOrDefault(t => t.X == current.X && t.Y == current.Y + y);
            if (tile != null && !tile.Equals(previous))
            {
                adjacent = tile;
                return true;
            }
        }

        adjacent = null;
        return false;
    }
    
    private bool IsInPolygon( Tile p, Tile[] poly)
    {
        Point p1, p2;
        bool inside = false;

        if (poly.Length < 3)
        {
            return inside;
        }

        var oldPoint = new Point(
            poly[poly.Length - 1].X, poly[poly.Length - 1].Y);

        for (int i = 0; i < poly.Length; i++)
        {
            var newPoint = new Point(poly[i].X, poly[i].Y);

            if (newPoint.X > oldPoint.X)
            {
                p1 = oldPoint;
                p2 = newPoint;
            }
            else
            {
                p1 = newPoint;
                p2 = oldPoint;
            }

            if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
                && (p.Y - (long) p1.Y)*(p2.X - p1.X)
                < (p2.Y - (long) p1.Y)*(p.X - p1.X))
            {
                inside = !inside;
            }

            oldPoint = newPoint;
        }

        return inside;
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
            return (X *100) + Y;
            return $"{X}{Y}".GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return ((Tile)this).GetHashCode() == ((Tile)obj).GetHashCode();
        }
    }
}