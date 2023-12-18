using System.Drawing;

namespace AOC23.Day18;

public class Quarry
{
    private List<DigInstruction> _instructions = new List<DigInstruction>();
    
    public long Part1(string input)
    {
        ParseInput(input);
        return 5;
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

    private enum Direction
    {
        R = 'R',
        L = 'L',
        U = 'U',
        D = 'D'
    }
}