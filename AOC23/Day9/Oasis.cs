namespace AOC23.Day9;

public class Oasis
{
    private List<Sequence> _sequences = new();
    
    public long Calculate(string input)
    {
        var total = 0;

        ParseInput(input);

        foreach (var sequence in _sequences)
        {
            sequence.Calculate();
        }

        //return _sequences.Sum(s => s.Numbers.Last());
        return _sequences.Sum(s => s.Numbers.First());
    }
    
    private void ParseInput(string input)
    {
        var lines = input.Split("\n").Where(l => !string.IsNullOrEmpty(l));
        foreach (var line in lines)
        {
            var seq = new Sequence();
            var parts = line.Split(" ");
            foreach (var part in parts)
            {
                seq.Numbers.Add(long.Parse(part));
            }
            _sequences.Add(seq);
        }
    }
    
    private class Sequence
    {
        public List<long> Numbers { get; set; } = new();

        public void Calculate()
        {
            var subSequence = GetSubSequence();
            if (!subSequence.IsEnd())
            {
                subSequence.Calculate();
            }
            
            //ExtrapolateNextDigit(subSequence);
            ExtrapolatePreviousDigit(subSequence);
        }
        
        private void ExtrapolateNextDigit(Sequence sub)
        {
            Numbers.Add(sub.Numbers.Last() + Numbers.Last());
        }

        private void ExtrapolatePreviousDigit(Sequence sub)
        {
            Numbers = Numbers.Prepend(Numbers.First() - sub.Numbers.First()).ToList();
        }

        private Sequence GetSubSequence()
        {
            var sub = new Sequence();
            for(int i = 1; i < Numbers.Count; i++)
            {
                sub.Numbers.Add(Numbers[i] - Numbers[i-1]);
            }

            return sub;
        }

        private bool IsEnd() => Numbers.All(n => n == 0);
    }
}