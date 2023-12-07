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
        {'J', 1},
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
        { 'T', 10 },
        //{ 'J', 10 },
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
        HC = 1
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
            var _ht = CalculateHandType(Cards);
            HandType = Upgrade(_ht, Cards.Count(c => c == 'J'));
        }

        public HandType Upgrade(HandType type, int numJokers)
        {
            var ct = type;
            for (int i = 0; i < numJokers; i++)
            {
                switch (ct)
                {
                    case HandType.HC:
                        ct = HandType.OP;
                        break;
                    case HandType.OP:
                        ct = HandType.TOAK;
                        break;
                    case HandType.TP:
                        ct = HandType.FH;
                        break;  
                    case HandType.TOAK:
                    case HandType.FH:
                        ct = HandType.FOOAK;
                        break;
                    case HandType.FOOAK:
                        ct = HandType.FIOAK;
                        break;
                }
            }

            return ct;
        }

        private static HandType CalculateHandType(char[] cards)
        {
            var testSet = cards.Where(c => c != 'J');
            var totalJokers = 5 - testSet.Count();
            
            if(testSet.Distinct().Count() + totalJokers == 4)
            {
                return HandType.OP;
            }
            
            if (testSet.Distinct().Count() == 2 && testSet.Count() == 5 && testSet.GroupBy(c => c).All(c => c.Count() < 4))
            {
                return HandType.FH;
            }
            
                        
            if(testSet.GroupBy(c => c).Count(c => c.Count() == 2) == 2)
            {
                return HandType.TP;
            }
            
            if(testSet.Distinct().Count() + totalJokers == 3)
            {
                return HandType.TOAK;
            }
            
            if (testSet.Distinct().Count() + totalJokers == 1)
            {
                return HandType.FIOAK;
            }


            
            if (testSet.Distinct().Count() + totalJokers == 2)
            {
                return HandType.FOOAK;
            }



            


            return HandType.HC;
        }

        public long GetScore()
        {
            var baseScore = (long)HandType ;
            return (long) baseScore;
        }
    }
}
