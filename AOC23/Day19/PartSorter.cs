using System.Text.RegularExpressions;

namespace AOC23.Day19;

public class PartSorter
{
    private Dictionary<string, RuleSet> _sets = new();
    
    private List<Part> _parts = new();

    public long CalculatePart1(string input)
    {
        ParseInput(input);

        foreach (var part in _parts)
        {
            Result? result = null;
            var currentRuleSet = "in";
            do
            {
                var set = _sets[currentRuleSet];
                foreach (var rule in set.Rules)
                {
                    result = rule.Function(part);
                    if(result.NextRuleSet != null)
                        currentRuleSet = result.NextRuleSet;
                    if (result.Accepted != null || result.Rejected != null || result.NextRuleSet != null)
                        break;
                }
                
                if(result.Accepted  == null && result.Rejected == null && result.NextRuleSet == null)
                    result = set.Final();
                
                if (result.NextRuleSet != null)
                    currentRuleSet = result.NextRuleSet;

                if (result.Accepted != null || result.Rejected != null)
                {
                    part.Accepted = result.Accepted == true;
                    break;
                }
            } while (result.Accepted == null && result.Rejected == null);
        }
        
        return _parts.Where(p => p.Accepted).Sum(p => p.X + p.M + p.A + p.S);
    }

    private void ParseInput(string input)
    {
        var lines = input.Split('\n');
        
        // First get rulesets
        var lineCount = 0;
        while (!string.IsNullOrEmpty(lines[lineCount]))
        {
            var ruleSet = new RuleSet
            {
                Rules = new List<Rule>()
            };
            
            var name = lines[lineCount].Substring(0, lines[lineCount].IndexOf('{'));
            var instructions = lines[lineCount].Substring(lines[lineCount].IndexOf('{') + 1, lines[lineCount].IndexOf('}') - lines[lineCount].IndexOf('{') - 1);
            var rulesRegex = new Regex(@"([xmas][><]\d+:[a-zAR]*)");
            var rulesMatches = rulesRegex.Matches(lines[lineCount]);
            foreach (Match rulesMatch in rulesMatches)
            {
                var group = rulesMatch.Groups[0];
                var rule = group.Value;
                ruleSet.Rules.Add(Rule.CreateFromString(rule));
            }

            var final = instructions.Split(',').Last();
            
            ruleSet.Final = () => new Result
            {
                Accepted = final == "A" ? true : null,
                Rejected = final == "R" ? true : null,
                NextRuleSet = final != "A" && final != "R" ? final : null
            };
            
            _sets.Add(name, ruleSet);
            lineCount++;
        }

        lineCount++;
        while (lineCount < lines.Length && !string.IsNullOrEmpty(lines[lineCount]))
        {
            var partsRegex = new Regex(@"{([x]=\d+),([m]=\d+),([a]=\d+),([s]=\d+)}");
            var partsMatches = partsRegex.Match(lines[lineCount]);
            var part = new Part
            {
                X = long.Parse(partsMatches.Groups[1].Value.Split('=').Last()),
                M = long.Parse(partsMatches.Groups[2].Value.Split('=').Last()),
                A = long.Parse(partsMatches.Groups[3].Value.Split('=').Last()),
                S = long.Parse(partsMatches.Groups[4].Value.Split('=').Last())
            };
            _parts.Add(part);
            lineCount++;
        }

        // Then parts
    }

    private class Part
    {
        public long X { get; set; }
        public long M { get; set; }
        public long A { get; set; }
        public long S { get; set; }
        
        public bool Accepted { get; set; }
    }

    private class RuleSet
    {
        public List<Rule> Rules { get; set; }
        public Func<Result> Final { get; set; }
    }

    private class Rule
    {
        public Func<Part, Result> Function { get; set; }

        public static Rule CreateFromString(string strRule)
        {
            var testProp = strRule.First().ToString().ToUpper(); // X, M, A, S
            var check = strRule[1].ToString(); // < or >
            var value = int.Parse(strRule.Substring(2, strRule.IndexOf(':') - 2)); // Value to check
            var path = strRule.Substring(strRule.IndexOf(':') + 1); // Next rule set to check
            bool? accepted = path == "A" ? true : null;
            bool? rejected = path == "R" ? true : null;

            if (accepted != null || rejected != null)
                path = null;
            
            return new Rule
            {
                Function = (p) =>
                {
                    var val = (long)p.GetType().GetProperty(testProp).GetValue(p, null);
                    if (check == "<")
                    {
                        if (val < value)
                        {
                            return new Result
                            {
                                Accepted = accepted,
                                Rejected = rejected,
                                NextRuleSet = path != "A" && path != "R" ? path : null
                            };
                        }
                        else
                        {
                            return new Result();
                        }
                    }
                    else
                    {
                        if (val > value)
                        {
                            return new Result
                            {
                                Accepted = accepted,
                                Rejected = rejected,
                                NextRuleSet = path != "A" && path != "R" ? path : null
                            };
                        }
                        else
                        {
                            return new Result();
                        }
                    }
                }
            };
        }
    }

    private class Result
    {
        public bool? Accepted { get; set; }
        public bool? Rejected { get; set; }
        public string? NextRuleSet { get; set; }
    }
}