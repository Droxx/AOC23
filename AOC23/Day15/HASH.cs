using System.IO.Pipes;

namespace AOC23.Day15;

public class HASH
{
    private List<string> _instructions = new List<string>();
    
    private Dictionary<int, Box> _boxes = new Dictionary<int, Box>();

    private Dictionary<int, byte> _lensPowers = new Dictionary<int, byte>();
    
    private long _resultSum = 0;

    public HASH()
    {
        for (int i = 0; i < 256; i++)
        {
            _boxes.Add(i, new Box());
        }
    }
    
    public long Calculate(string input)
    {
        ParseInput(input);
        foreach (var instructon in _instructions)
        {
            _resultSum += HashInstruction(instructon);
        }

        return _resultSum;
    }

    private void ProcessInstruction(string instruction)
    {
        // For each instruction
        
        // First string is the LABEL of the lens on which the step operates
        // The result of hashing that LABEL is the box fot that step
        
        // Then = or -
        
        // ON -
        // Go to the box and remove the lens with the LABEL
        // Move any other lenses in that box as far forward as they can go (without changin order)
        
        // ON = (will be followed by a number)
        // Numebr is focal length
        // We may only see this once, so make sure to associate that label with the focal length for future use
        // IF box contains a lens with that LABEL, remove the old lens, and replace with the new focal length
        // IF the box does not contain a lens with that LABEL, then add that lens BEHIND all other lenses in box, don't move any other lenses
        
    }

    private long CalculateFocusingPower()
    {
        foreach (var box in _boxes)
        {
            var boxInt = 1 + box.Key;
            var runningValueOfBox = 0;
            for (int s = 0; s < box.Value.Lenses.Count; s++)
            {
                var lensVal = boxInt * (1 + s) * box.Value.Lenses[s].FocalLength;
                runningValueOfBox += lensVal;
            }

            box.Value.FocusingPower = runningValueOfBox;
        }

        return _boxes.Values.Sum(b => b.FocusingPower);
    }
    

    private long HashInstruction(string instruction)
    {
        var currentValue = 0;
        foreach (var ch in instruction.TrimEnd())
        {
            var chInt = (int)ch;
            currentValue += chInt;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    private void ParseInput(string input)
    {
       _instructions = input.Split(',').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
    }

    private class Box
    {
        public List<Lens> Lenses { get; set; } = new();
        
        public long FocusingPower { get; set; }

    }

    private class Lens
    {
        public string LensCode { get; set; }
        public int FocalLength { get; set; } // 1-9
        public string Hash { get; set; }
        
    }
}