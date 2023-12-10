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
            
            /*
            if(AnyAdjacentVisitedBefore(currentA, previousA, out var adjacent))
            {
                var polygon = new List<Tile>();
                for (int i = _visitedTiles.Count()-1; i >= _visitedTiles.IndexOf(adjacent); i--)
                {
                    polygon.Add(_visitedTiles.ElementAt(i));
                }

                var polyMaxX = polygon.Max(p => p.X);
                var polyMinX = polygon.Min(p => p.X);
                var polyMaxY = polygon.Max(p => p.Y);
                var polyMinY = polygon.Min(p => p.Y);
                
                for (int y = polyMinY; y < polyMaxY; y++)
                {
                    for (int x = polyMinX; x < polyMaxX; x++)
                    {
                        var point = _map.FirstOrDefault(t => t.X == x && t.Y == y);
                        if(!_visitedTiles.Contains(point) && !_encompassed.Contains(point) && IsInPolygon(_map.FirstOrDefault(t => t.X == x && t.Y == y), polygon.ToArray()))
                        {
                            _encompassed.Add(point);
                        }
                    }
                }
            }
            */
            
            
            
            _visitedTiles.Add(currentA);
            
            Console.Write($"On step {steps} of ~14000\r");
            // For two runners in oposite directions
            //var newB = FollowPipe(currentB, previousB);
            //previousB = currentB;
            //currentB = newB;

            // For debug printing
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
        } while (currentA != startPosition);

        Console.Write("\n\r");
        Console.Write("\n\r");

        for (int y = 0; y < lines.Count(); y++)
        {
            for (int x =  0; x < lines.ElementAt(y).Count(); x++)
            {
                if (_visitedTiles.Any(v => v.X == x && v.Y == y))
                {
                    Console.Write(lines.ElementAt(y)[x]);

                }
                else if (_encompassed.Any(v => v.X == x && v.Y == y))
                {
                    Console.Write('I');
                }
                else
                {
                    Console.Write(lines.ElementAt(y)[x]);

                    //Console.Write('O');
                }
            }
            Console.Write("\n\r");
        }
_visitedTiles.Add(startPosition);
        var area = Math.Abs(_visitedTiles.Take(_visitedTiles.Count - 1)
            .Select((p, i) => (_visitedTiles[i + 1].X - p.X) * (_visitedTiles[i + 1].Y + p.Y))
            .Sum() / 2);
        
        /*foreach (var vi in _encompassed)
        {
            if (!_visitedTiles.Contains(vi))
            {
                area++;10
            }
        }*/
        
        return area;
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