using System.Drawing;

namespace AOC23.Day18;

public class Quarry
{
    private List<DigInstruction> _instructions = new List<DigInstruction>();
    private List<Coordinates> _visitedPoints = new List<Coordinates>();
    private List<Coordinates> _polygon = new List<Coordinates>();

    public long Part1(string input)
    {
        ParseInput(input);
        ExecuteInstructions();
        _visitedPoints.Remove(_visitedPoints.Last());

        //var area = CalculateDigArea();
        
        //TransposeRows();
        
        PrintGrid();

        var area = Math.Abs(_polygon.Take(_polygon.Count - 1)
            .Select((p, i) => (_polygon[i + 1].X - p.X) * (_polygon[i + 1].Y + p.Y))
            .Sum() / 2);

        return (long)area;
    }
    
    
    public void TransposeRows()
    {
        var newList = new List<Coordinates>();
        for (var x = 0; x <= _visitedPoints.Max(v => v.X); x++)
        {
            for (var y = 0; y <= _visitedPoints.Max(v => v.Y); y++)
            {
                if(_visitedPoints.Any(p => p.X == x && p.Y == y))
                {
                    newList.Add(new Coordinates
                    {
                        X = y,
                        Y = x
                    });
                }
            }
        }

        _visitedPoints = newList;
    }

    private void PrintGrid()
    {
        for (var y = 0; y <= _visitedPoints.Max(p => p.Y); y++)
        {
            for (var x = 0; x <= _visitedPoints.Max(p => p.X); x++)
            {
                if(_visitedPoints.Any(p => p.X == x && p.Y == y))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }

            Console.WriteLine();
        }
    }
    
    private long CalculateDigAreaPointInPolygon()
    {
        var poly = _visitedPoints.ToArray();
        long area = 0;
        for (var y = 0; y < _visitedPoints.Max(p => p.Y); y++)
        {
            for (var x = 0; x < _visitedPoints.Max(p => p.X); x++)
            {
                var coords = new Coordinates() { X = x, Y = y };
                if (IsInPolygon(coords, poly))
                {
                    area++;
                }
            }
        }

        return area + _visitedPoints.Count;
    }

    private long CalculateDigArea()
    {
        var inside = false;
        long runningCount = 0;
        for (var y = 0; y <= _visitedPoints.Max(p => p.Y); y++)
        {
            for(var x = 0; x <= _visitedPoints.Max(p => p.X); x++)
            {
                var point = _visitedPoints.FirstOrDefault(p => p.X == x && p.Y == y);
                if (point != null)
                {
                    runningCount++;
                    
                    var adjacentL = _visitedPoints.FirstOrDefault(p => p.X == x - 1 && p.Y == y);
                    var adjacentR = _visitedPoints.FirstOrDefault(p => p.X == x + 1 && p.Y == y);
                    
                    if (!point.IsHorizontalLine || (adjacentL == null || !adjacentL.IsHorizontalLine) || (adjacentR == null || !adjacentR.IsHorizontalLine))
                    {
                        inside = !inside;
                    }
                }
                else if (inside)
                {
                    runningCount++;
                }
            }
        }

        return runningCount;
    }
    
    private bool IsInPolygon(Coordinates p, Coordinates[] poly)
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

    private void ExecuteInstructions()
    {
        var currentX = 0;
        var currentY = 0;
        _visitedPoints.Add(new Coordinates {X = currentX, Y = currentY, IsHorizontalLine = _instructions[0].Direction == Direction.L || _instructions[0].Direction == Direction.R});
        _polygon.Add(new Coordinates
        {
            X = currentX,
            Y = currentY,
        });
        foreach (var instruction in _instructions)
        {

            for(var i = 0; i < instruction.Distance; i++)
            {
                switch (instruction.Direction)
                {
                    case Direction.R:
                        currentX++;
                        break;
                    case Direction.L:
                        currentX--;
                        break;
                    case Direction.U:
                        currentY--;
                        break;
                    case Direction.D:
                        currentY++;
                        break;
                }

                _visitedPoints.Add(new Coordinates {X = currentX, Y = currentY, IsHorizontalLine = instruction.Direction == Direction.L || instruction.Direction == Direction.R});
            }
            _polygon.Add(new Coordinates
            {
                X = currentX,
                Y = currentY,
            });
        }
    }
    
    private void ParseInput(string input)
    {
        var lines = input.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        foreach (var line in lines)
        {
            var instruction = new DigInstruction();
            var parts = line.Split(" ");
            instruction.Direction = (Direction)parts[0][0];
            instruction.Distance = int.Parse(parts[1]);
            var hexStr = parts[2].TrimStart('(').TrimEnd(')');
            int r = int.Parse(hexStr.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(hexStr.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(hexStr.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            instruction.Color = Color.FromArgb(r, g, b);
            _instructions.Add(instruction);
        }
    }

    private class DigInstruction
    {
        public Direction Direction { get; set; }
        public int Distance { get; set; }
        public Color Color { get; set; }
    }

    private class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsHorizontalLine { get; set; }

        public override int GetHashCode()
        {
            return (X*1000) + Y;
        }
    }
    
    private enum Direction
    {
        R = 'R',
        L = 'L',
        U = 'U',
        D = 'D'
    }
}