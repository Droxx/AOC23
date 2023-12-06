using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AOC23.Day4;

public class Scratchcards
{
    private class Card
    {
        public int CardId { get; set; }
        public int Count { get; set; } = 1;
        public List<int> WinningNums { get; set; }
        public List<int> ScratchedNums { get; set; }
        
        public int CalculateWinnings()
        {
            var winnings = 0;
            foreach (var num in ScratchedNums)
            {
                if (WinningNums.Contains(num))
                {
                    //winnings = winnings == 0 ? 1 : winnings * 2;
                    winnings++;
                }
            }

            return winnings;
        }
    }
    
    private List<Card> _cards = new();

    public int CalculateWinnings(string input)
    {
        var lines = input.Split("\n").Where(l => !string.IsNullOrEmpty(l));

        foreach (var line in lines)
        {
            var regex = new Regex(@"Card\s+(\d*): ([\d ]*)| ([\d ]*)");
            var matches = regex.Matches(line);
            var cardNum = int.Parse(matches[0].Groups[1].Value);
            var winningNums = matches[0].Groups[2].Value.Split(" ").Where(n => int.TryParse(n, out var p)).Select(n => int.Parse(n)).ToList();
            var scratchedNums = matches[1].Groups[3].Value.Split(" ").Where(n => int.TryParse(n, out var p)).Select(n => int.Parse(n)).ToList();
            
            _cards.Add(new Card
            {
                CardId = cardNum,
                WinningNums = winningNums,
                ScratchedNums = scratchedNums
            });
        }

        foreach (var card in _cards)
        {
            ProcessCard(_cards.IndexOf(card));
        }

        return _cards.Sum(c => c.Count);
    }

    private void ProcessCard(int index)
    {
        var card = _cards[index];
        var winnings = card.CalculateWinnings();
        
        if (winnings == 0)
        {
            return;
        }
        
        for(int i = 1; i <= winnings; i++)
        {
            if(i < _cards.Count)
            {
                _cards[index + i].Count++;
                ProcessCard(index + i);
            }
        }

        return;
    }
}