namespace AOC23.Day7;

public class CamelCards
{
    private List<Hand> _hands = new();
    
    public long Calculate(string input)
    {
        var lines = input.Split("\n").Where(l => !string.IsNullOrEmpty(l));

        foreach (var line in lines)
        {
            var parts = line.Split(" ");
            var bid = long.Parse(parts[1]);
            _hands.Add(new Hand(parts[0], bid));
        }
        
        var orderedHands = _hands
            .OrderByDescending(h => h.GetScore())
            .ThenByDescending(h => _cardVals[h.Cards[0]])
            .ThenByDescending(h => _cardVals[h.Cards[1]])
            .ThenByDescending(h => _cardVals[h.Cards[2]])
            .ThenByDescending(h => _cardVals[h.Cards[3]])
            .ThenByDescending(h => _cardVals[h.Cards[4]])
            .ToList();

        long result = 0;
        for (var i = 0; i < orderedHands.Count; i++)
        {
            result += orderedHands[i].Bid * (orderedHands.Count() - i);
        }

        return result;
    }
    
    
    private static Dictionary<char, int> _cardVals = new Dictionary<char, int>
    {
        { '2', 1 },
        { '3', 2 },
        { '4', 3 },
        { '5', 4 },
        { '6', 5 },
        { '7', 6 },
        { '8', 7 },
        { '9', 8 },
        { 'T', 9 },
        { 'J', 10 },
        { 'Q', 11 },
        { 'K', 12 },
        { 'A', 13 }
    };

    private enum HandType
    {
        FIOAK = 7,
        FOOAK = 6,
        FH = 5,
        TOAK = 4,
        TP = 3,
        OP = 2,
        P = 1
    }

    private class Hand
    {
        public long Bid { get; private set; }
        public char[] Cards { get; private set; }
        public HandType HandType { get; private set; }
        
        
        public Hand(string input, long bid)
        {
            if(input.Length > 5)
            {
                throw new Exception("Too many cards");
            }
            Cards = input.ToCharArray();
            Bid = bid;
            HandType = CalculateHandType(Cards);
        }

        private static HandType CalculateHandType(char[] cards)
        {
            if (cards.Distinct().Count() == 1)
            {
                return HandType.FIOAK;
            }

            if (cards.Distinct().Count() == 2 && cards.GroupBy(c => c).Any(g => g.Count() == 3))
            {
                return HandType.FH;
            }
            
            if (cards.Distinct().Count() == 2)
            {
                return HandType.FOOAK;
            }
            
            if(cards.Distinct().Count() == 3 && cards.GroupBy(c => c).Any(g => g.Count() == 2))
            {
                return HandType.TP;
            }

            if(cards.Distinct().Count() == 3)
            {
                return HandType.TOAK;
            }
            
            if(cards.Distinct().Count() == 2)
            {
                return HandType.OP;
            }

            return HandType.P;
        }

        public long GetScore()
        {
            var baseScore = (long)HandType * 1000000000000;
            
            var additional = 0;
            /*
            for(int i = 0; i < Cards.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        additional += _cardVals[Cards[i]] * 1000000000;
                        break;
                    case 1:
                        additional += _cardVals[Cards[i]] * 1000000;
                        break;
                    case 2:
                        additional += _cardVals[Cards[i]] * 10000;
                        break;
                    case 3:
                        additional += _cardVals[Cards[i]] * 100;
                        break;
                    case 4:
                        additional += _cardVals[Cards[i]];
                        break;
                }

            }
            */

            return (long) baseScore + additional;
        }
    }
}