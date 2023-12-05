namespace AOC23.Day1;

public class CalibrationValues
{
    private readonly string _input;
    
    public CalibrationValues(string input)
    {
        _input = input;
    }

    public int Compute()
    {
        var lines = _input.Split("\n");
        var total = 0;

        foreach (var line in lines.Where(l => l.Length > 0))
        {
            total += GetTotal(line);
        }

        return total;
    }

    private int GetTotal(string line)
    {
        int? first = null;
        int? last = null;
        for (var i = 0; i < line.Length; i++)
        {
            var sub = line.Substring(i);
            var digit = GetStrDigit(sub);
            if (digit != null)
            {
                if (first == null)
                {
                    first = digit.Item1;
                }
                else
                {
                    last = digit.Item1;
                }
            }
            else
            {
                if (int.TryParse(line[i].ToString(), out var dig))
                {
                    if (first == null)
                    {
                        first = dig;
                    }
                    else
                    {
                        last = dig;
                    }
                }
            }
        }
        return int.Parse($"{first}{last ?? first}");
    }

    private Tuple<int, int>? GetStrDigit(string sub)
    {
        if (sub.StartsWith("one"))
        {
            return new Tuple<int, int>(1,3);
        }
        else if (sub.StartsWith("two"))
        {
            return new Tuple<int, int>(2,3);
        }
        else if (sub.StartsWith("three"))
        {
            return new Tuple<int, int>(3,5);
        }
        else if (sub.StartsWith("four"))
        {
            return new Tuple<int, int>(4,4);
        }
        else if (sub.StartsWith("five"))
        {
            return new Tuple<int, int>(5,4);
        }
        else if (sub.StartsWith("six"))
        {
            return new Tuple<int, int>(6,3);
        }
        else if (sub.StartsWith("seven"))
        {
            return     new Tuple<int, int>(7,5);
        }
        else if (sub.StartsWith("eight"))
        {
            return new Tuple<int, int>(8,5);
        }
        else if (sub.StartsWith("nine"))
        {
            return new Tuple<int, int>(9,4);
        }

        return null;
    }
}