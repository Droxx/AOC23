namespace AOC23.Day16;

public class Beams
{
    // [Y,X]
    private Tile[,] _grid;
    
    public long Calculate(string input)
    {
        ParseInput(input);
        
        //Energise(0,0,1,0);

        // Sum every record in _grid, 1 if energised, 0 if not
        //return GetSum();
        
        FindBestDirection();
        return _currentMax;
    }

    private int _currentMax = 0;
    
    private void FindBestDirection()
    {
        for(int y = 0; y < _grid.GetLength(0); y++)
        {
            Energise(0, y, 1, 0);
            var sum = GetSum();
            if (sum > _currentMax)
            {
                _currentMax = sum;
            }
            ResetGrid();
            
            Energise(_grid.GetLength(1)-1, y, -1, 0);
            sum = GetSum();
            if (sum > _currentMax)
            {
                _currentMax = sum;
            }
            ResetGrid();
        }
        
        for(int x = 0; x < _grid.GetLength(1); x++)
        {
            Energise(x, 0, 0, 1);
            var sum = GetSum();
            if (sum > _currentMax)
            {
                _currentMax = sum;
            }
            ResetGrid();
            
            Energise(x, _grid.GetLength(0)-1, 0, -1);
            sum = GetSum();
            if (sum > _currentMax)
            {
                _currentMax = sum;
            }
            ResetGrid();
        }
    }

    private int GetSum()
    {
        var sum = 0;
        for (var y = 0; y < _grid.GetLength(0); y++)
        {
            for (var x = 0; x < _grid.GetLength(1); x++)
            {
                if (_grid[y, x].IsEnergised)
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    private void ResetGrid()
    {
        for (var y = 0; y < _grid.GetLength(0); y++)
        {
            for (var x = 0; x < _grid.GetLength(1); x++)
            {
                _grid[y, x].Reset();
            }
        }
    }

    private void PrintGrid(int? startY, int? startX)
    {
        Console.WriteLine();
        for (var y = 0; y < _grid.GetLength(0); y++)
        {
            for (var x = 0; x < _grid.GetLength(1); x++)
            {
                if (startX != null && startY != null)
                {
                    if (x == startX && y == startY)
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write((char)_grid[y, x].Type);
                    }
                }
                else
                {
                    if (_grid[y, x].IsEnergised)
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write((char)_grid[y, x].Type);
                    }
                }
            }

            Console.WriteLine();

        }
    }

    private void Energise(int startX, int startY, int dirX, int dirY)
    {
        // If we're at an edge, return, we have nowhere else to go
        if (startX >= _grid.GetLength(1) || startX < 0)
        {
            return;
        }
        if(startY >= _grid.GetLength(0) || startY < 0)
        {
            return;
        }
        
        // Energize the current tile
        var currentTile = _grid[startY, startX];

        // If we already energised this way, cancel, otherwise we get infinite loops
        if (currentTile.HasEnergizedInThisDir(dirX, dirY))
        {
            return;
        }
        else
        {
            currentTile.Energize(dirX, dirY);
        }
        //PrintGrid(startY, startX);
        if (Math.Abs(dirX) > 0)
        {
            switch (currentTile.Type)
            {
                case Tile.TileType.Neutral:
                case Tile.TileType.HorizontalSplitter:
                    Energise(startX+dirX, startY+dirY, dirX, dirY);
                    break;
                case Tile.TileType.VertialSplitter:
                    Energise(startX, startY+1, 0, 1);
                    Energise(startX, startY-1, 0, -1);
                    break;
                case Tile.TileType.MirrorA: // /
                    Energise(startX, startY-(dirX), 0, -(dirX));
                    break;
                case Tile.TileType.MirrorB: // \
                    Energise(startX, startY+(dirX), 0, (dirX));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else if (Math.Abs(dirY) > 0)
        {
            switch (currentTile.Type)
            {
                case Tile.TileType.Neutral:
                case Tile.TileType.VertialSplitter:
                    Energise(startX+dirX, startY+dirY, dirX, dirY);
                    break;
                case Tile.TileType.HorizontalSplitter:
                    Energise(startX+1, startY, 1, 0);
                    Energise(startX-1, startY, -1, 0);
                    break;
                case Tile.TileType.MirrorA: // /
                    Energise(startX-dirY, startY, -dirY, 0);
                    break;
                case Tile.TileType.MirrorB: // \
                    Energise(startX+dirY, startY, +dirY, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
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
        
        public bool EnergizedYUp { get; set; }
        public bool EnergizedYDown { get; set; }
        public bool EnergizedXLeft { get; set; }
        public bool EnergizedXRight { get; set; }

        public bool HasEnergizedInThisDir(int dirX, int dirY)
        {
            if (IsEnergised)
            {
                if(dirX > 0 && EnergizedXRight)
                {
                    return true;
                }
                if(dirX < 0 && EnergizedXLeft)
                {
                    return true;
                }
                if(dirY > 0 && EnergizedYDown)
                {
                    return true;
                }
                if(dirY < 0 && EnergizedYUp)
                {
                    return true;
                }
            }

            return false;
        }
        
        public void Energize(int dirX, int dirY)
        {
            IsEnergised = true;
            if(dirX > 0)
            {
                EnergizedXRight = true;
            }
            else if(dirX < 0)
            {
                EnergizedXLeft = true;
            }
            else if(dirY > 0)
            {
                EnergizedYDown = true;
            }
            else if(dirY < 0)
            {
                EnergizedYUp = true;
            }
        }

        public void Reset()
        {
            IsEnergised = false;
            EnergizedXLeft = false;
            EnergizedXRight = false;
            EnergizedYUp = false;
            EnergizedYDown = false;
        }

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