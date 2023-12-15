namespace AOC23.Day15;

public class HASH
{
    private List<string> _instructions = new List<string>();
    private long _resultSum = 0;
    
    public long Calculate(string input)
    {
        ParseInput(input);
        foreach (var instructon in _instructions)
        {
            _resultSum += HashInstruction(instructon);
        }

        return _resultSum;
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
}