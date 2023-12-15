using System.IO.Pipes;
using System.Text.RegularExpressions;

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
            ProcessInstruction(instructon);
        }

        return CalculateFocusingPower();
    }

    private void ProcessInstruction(string instruction)
    {
        // For each instruction

        // First string is the LABEL of the lens on which the step operates
        // The result of hashing that LABEL is the box fot that step
        var regex = new Regex(@"([a-z]+)[=|-|,]*");
        var matches = regex.Matches(instruction);
        var label = matches[0].Groups[1].Value;
        
        var hash = Hash(label);
        var box = _boxes[hash];

        var remaining = instruction.Replace(label, "");
        if (!remaining.Any())
        {
            // NO INSTRUCTION AFTER, LOOK UP IN LENS TABLE
            throw new Exception("Should this happen?");
            // Looking over the data it seems this may never be the case
        }
        else if (remaining[0] == '-')
        {
            // ON -
            // Go to the box and remove the lens with the LABEL
            // Move any other lenses in that box as far forward as they can go (without changin order)
            if (box.Lenses.TryGetValue(label, out var existingLens))
            {
                var slotNoOfLens = existingLens.SlotNumber;
                box.Lenses.Remove(label);
                foreach (var subsequentLens in box.Lenses.Values.Where(l => l.SlotNumber > slotNoOfLens))
                {
                    subsequentLens.SlotNumber--;
                }
            }
        }
        else if(remaining[0] == '=')
        {
            // ON = (will be followed by a number)
            // Number is focal length

            var focalLength = int.Parse(remaining.Substring(1));

            if (box.Lenses.TryGetValue(label, out var existingLens))
            {
                // IF box contains a lens with that LABEL, remove the old lens, and replace with the new focal length
                existingLens.FocalLength = focalLength;
            }
            else
            {
                // IF the box does not contain a lens with that LABEL, then add that lens BEHIND all other lenses in box, don't move any other lenses
                var currentHighSlotNo = box.Lenses.Any() ? box.Lenses.Max(l => l.Value.SlotNumber) : 0;
                box.Lenses.Add(label, new Lens
                {
                    FocalLength = focalLength,
                    SlotNumber = currentHighSlotNo + 1
                });
            }
            
            // We may only see this once, so make sure to associate that label with the focal length for future use
        }
    }

    private long CalculateFocusingPower()
    {
        foreach (var box in _boxes)
        {
            var boxInt = 1 + box.Key;
            var runningValueOfBox = 0;
            foreach (var lens in box.Value.Lenses)
            {
                var lensVal = boxInt * lens.Value.SlotNumber * lens.Value.FocalLength;
                runningValueOfBox += lensVal;
            }

            box.Value.FocusingPower = runningValueOfBox;
        }

        return _boxes.Values.Sum(b => b.FocusingPower);
    }
    

    private byte Hash(string instruction)
    {
        var currentValue = 0;
        foreach (var ch in instruction.TrimEnd())
        {
            var chInt = (int)ch;
            currentValue += chInt;
            currentValue *= 17;
            currentValue %= 256;
        }

        return (byte)currentValue;
    }

    private void ParseInput(string input)
    {
       _instructions = input.Split(',').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
    }

    private class Box
    {
        public Dictionary<string, Lens> Lenses { get; set; } = new();
        public long FocusingPower { get; set; }

    }

    private class Lens
    {
        public int SlotNumber { get; set; }
        public int FocalLength { get; set; } // 1-9
        
    }
}