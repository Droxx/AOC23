namespace AOC23.Day12;

public class SpringSchematics
{
    private List<Line> _lines = new();
    
    public long Calculate(string input)
    {
        ParseInput(input);

        long possibilities = 0;

        int finishedLines = 0;
        
        
        Parallel.ForEach(_lines, l =>
        {
            var firstAns = Possibilities(l.RawRange, l.Summaries);
            var secondPart = Possibilities(l.RawRangePart2, l.Summaries);

            var secondPartExp = Math.Pow(secondPart, 4);
            l.Possibilities = firstAns * (long)secondPartExp;
            
            Interlocked.Increment(ref finishedLines);
            Console.WriteLine($"Completed {finishedLines} of {_lines.Count}");
        });
        
        /*foreach (var line in _lines)
        {
            lineCount++;
            Console.WriteLine($"Line {lineCount}");
            possibilities += Possibilities(line.RawRange, line.Summaries);
        }*/

        return _lines.Sum(l => l.Possibilities);
    }

    private Dictionary<int, long> _cached = new();

    public long Possibilities(string line, List<int> remainingGroups)
    {

        
        // If we are at the end of a line and there are no more groups, then this is a possible combo
        // Otherwise it's a failure, so return 0
        if (line.Length == 0)
        {
            if (remainingGroups.Count == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        
        // If it's a functional pump, remove that character and recurse
        if (line[0] == '.')
        {
            return Possibilities(line.Substring(1), remainingGroups);
        }
        
        // If it's unknown, we need to diverge, two possibilities, two recursive paths
        if (line[0] == '?')
        {
            var runKey = line.GetHashCode() * remainingGroups.GetHashCode();
            if (_cached.ContainsKey(runKey))
            {
                return _cached[runKey];
            }
            
            long result = 0;
            
            var functionalLine = line.ToCharArray();
            functionalLine[0] = '.';

            result += Possibilities(new string(functionalLine), remainingGroups);
           // Console.WriteLine($"{result}\t{remainingGroups.Count}\t{new string(functionalLine)}");
            
            var defectiveLine = line.ToCharArray();
            defectiveLine[0] = '#';
            
            result += Possibilities(new string(defectiveLine), remainingGroups);
           //Console.WriteLine($"{result}\t{remainingGroups.Count}\t{new string(defectiveLine)}");

            _cached.Add(runKey, result);
           
            return result;
        }

        // If it's defective, we now need to check it against the remaining groups
        // If it matches a group length, then we can remove that group from both the string and the remaining groups.
        // Then recurse further
        if (line[0] == '#')
        {
            if (remainingGroups.Any())
            {
                var nextGroup = remainingGroups.FirstOrDefault();
                if (nextGroup <= line.Length)
                {
                    var potential = line.Substring(0, nextGroup);
                    if (potential.All(p => p != '.') && (line.Length == nextGroup || line[nextGroup] != '#'))
                    {
                        if (line.Length > nextGroup && line[nextGroup] == '?')
                        {
                            return Possibilities(line.Substring(nextGroup+1), remainingGroups.Skip(1).ToList());
                        }
                        return Possibilities(line.Substring(nextGroup), remainingGroups.Skip(1).ToList());
                    }
                }
            }

            // We have a defective pump but run out of groups,
            // or the next group is too long,
            // or the line does not contain a group long enough, so cannot possibly match
            // we failed, drop out of recursion
            return 0;
        }

        throw new Exception("Encountered an unexpected character");
    }

    private void ParseInput(string input)
    {
        var lines = input.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

        foreach (var line in lines)
        {
            var ranges = line.Split(' ')[0];
            var summaries = line.Split(' ')[1];

            
            var qsatend = ranges.Length - ranges.TrimEnd('?').Length;
            
            
            var part2Ranges = '?' + new string('?', qsatend) + ranges;
            

            //var part2Ranges = '?' + ranges;
            
            var l = new Line
            {
                RawRange = ranges,
                RawRangePart2 = part2Ranges,
            };

            foreach (var summary in summaries.Split(','))
            {
                l.Summaries.Add(int.Parse(summary));
            }
            
            _lines.Add(l);
        }
    }

    private class Line
    {
        public string RawRange { get; set; }
        public string RawRangePart2 { get; set; }
        public List<int> Summaries { get; set; } = new();
        public long Possibilities { get; set; }

        public bool DoesSatisfySummary(char[] range)
        {
            var parsed = ParseLine(range);
            if(parsed.Count(p => p.Known) != Summaries.Count)
                return false;

            for (var i = 0; i < Summaries.Count; i++)
            {
                if(Summaries[i] != parsed.Where(p => p.Known && !p.Operational).ToList()[i].Length)
                    return false;
            }

            return true;
        }
        
        public static List<SpringRange> ParseLine(char[] range)
        {
            var parsed = new List<SpringRange>();
            for (var i = 0; i < range.Length; i++)
            {
                var rangeLen = 0;
                var isKnown = false;
                var currentChar = range[i];
                bool isOperational = false;
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
                        break;
                }
                
                do
                {
                    rangeLen++;
                } while (i+rangeLen < range.Length && range[i + rangeLen] == currentChar);

                i += rangeLen - 1;
                parsed.Add(new SpringRange{Known = isKnown, Operational = isOperational, Length = rangeLen});
            }

            return parsed;
        }
    }

    public class SpringRange
    {
        public int Length { get; set; }
        public bool Known { get; set; }
        public bool Operational { get; set; }
    }
}