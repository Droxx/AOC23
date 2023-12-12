namespace AOC23.Day12;

public class SpringSchematics
{
    private List<Line> _lines = new();
    
    public long Calculate(string input)
    {
        ParseInput(input);

        return 5;
    }

    private void ParseInput(string input)
    {
        var lines = input.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        foreach (var line in lines)
        {
            var ranges = line.Split(' ')[0];
            var summaries = line.Split(' ')[1];
            var l = new Line
            {
                Length = ranges.Length
            };
            
            for (var i = 0; i < ranges.Length; i++)
            {
                var rangeLen = 0;
                var isKnown = false;
                var currentChar = ranges[i];
                bool? isOperational = false;
                switch (currentChar)
                {
                    case '#':
                        isKnown = true;
                        isOperational = false;
                        break;
                    case '.':
                        isKnown = true;
                        isOperational = true;
                        break;
                    case '?':
                        isKnown = false;
                        isOperational = null;
                        break;
                }
                
                do
                {
                    rangeLen++;
                } while (i+rangeLen < ranges.Length && ranges[i + rangeLen] == currentChar);

                i += rangeLen - 1;
                l.Ranges.Add(new SpringRange{Known = isKnown, Operational = isOperational, Length = rangeLen});
            }

            foreach (var summary in summaries.Split(','))
            {
                l.Summaries.Add(int.Parse(summary.ToString()));
            }
            _lines.Add(l);
        }
    }

    private class Line
    {
        public int Length { get; set; }
        public List<SpringRange> Ranges { get; set; } = new();
        public List<int> Summaries { get; set; } = new();
    }

    public class SpringRange
    {
        public int Length { get; set; }
        public bool Known { get; set; }
        public bool? Operational { get; set; }
    }
}