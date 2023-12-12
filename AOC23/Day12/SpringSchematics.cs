﻿using System.Collections.Concurrent;

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
            Console.WriteLine($"Line {_lines.IndexOf(l)} START");
            l.Possibilities = Possibilities(l.RawRange, l.Summaries);
            Console.WriteLine($"Line {_lines.IndexOf(l)} END");
            Interlocked.Increment(ref finishedLines);
            Console.WriteLine($"Completed {finishedLines} of {_lines.Count}");
        });
        
        
        /*foreach (var line in _lines)
        {
            line.Possibilities = Possibilities(line.RawRange, line.Summaries);
            Interlocked.Increment(ref finishedLines);
            Console.WriteLine($"Completed {finishedLines} of {_lines.Count}");
        }*/

        return _lines.Sum(l => l.Possibilities);
    }
    
    private int GetHashCode(int[] values)
    {
        int result = 0;
        int shift = 0;
        for (int i = 0; i < values.Length; i++)
        {
            shift = (shift + 11) % 21;
            result ^= (values[i]+1024) << shift;
        }
        return result;
    }

    private ConcurrentDictionary<string, int> _cache = new();

    public int Possibilities(string line, List<int> remainingGroups)
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
            string runKey = line + string.Join(",", remainingGroups);

            if (_cache.TryGetValue(runKey, out var res))
            {
                return res;
            }
            
            var result = 0;
            
            var functionalLine = line.ToCharArray();
            functionalLine[0] = '.';

            result += Possibilities(new string(functionalLine), remainingGroups);
            //Console.WriteLine($"{result}\t{remainingGroups.Count}\t{new string(functionalLine)}");
            
            var defectiveLine = line.ToCharArray();
            defectiveLine[0] = '#';
            
            result += Possibilities(new string(defectiveLine), remainingGroups);
            //Console.WriteLine($"{result}\t{remainingGroups.Count}\t{new string(defectiveLine)}");

            try
            {
                _cache.TryAdd(runKey, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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
                        var result = 0;
                        if (line.Length > nextGroup && line[nextGroup] == '?')
                        {
                            result = Possibilities(line.Substring(nextGroup+1), remainingGroups.Skip(1).ToList());
                        }
                        else if(line.Length > nextGroup)
                        {
                            result = Possibilities(line.Substring(nextGroup+1), remainingGroups.Skip(1).ToList());
                        }
                        else
                        {
                            result = Possibilities(line.Substring(nextGroup), remainingGroups.Skip(1).ToList());
                        }

                        return result;
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

            var expanded = "";
            for (int i = 0; i < 5; i++)
            {
                expanded += ranges + "?";
            }
            // remove one character from end
            expanded = expanded.Substring(0, expanded.Length - 1);

            var l = new Line
            {
                Length = expanded.Length,
                RawRange = expanded,
                Ranges = Line.ParseLine(expanded.ToCharArray())
            };
            
            for (int i = 0; i <5; i++)
            {
                foreach (var summary in summaries.Split(','))
                {
                    l.Summaries.Add(int.Parse(summary));
                }
            }
            Console.WriteLine(expanded);
            
            _lines.Add(l);
        }
    }

    private class Line
    {
        public int Length { get; set; }
        public string RawRange { get; set; }
        public string RawRangePart2 { get; set; }
        public List<SpringRange> Ranges { get; set; } = new();
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