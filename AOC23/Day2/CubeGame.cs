namespace AOC23.Day2;

public class CubeGame
{
    private readonly string _input;
    public CubeGame(string input)
    {
        _input = input;
    }

    public int Play(int red, int blue, int green)
    {
        var lines = _input.Split("\n");
        var total = 0;
        foreach (var line in lines.Where(l => !string.IsNullOrEmpty(l)))
        {
            var possible = true;
            // Use a regex to extract the game number from the string containing "Game 1: "
            var gameNumberStr = line.Substring(4, line.IndexOf(":", StringComparison.Ordinal) - 4);
            var gameNumber = int.Parse(gameNumberStr);
            var gameStr = line.Substring(line.IndexOf(": ", StringComparison.Ordinal));

            var rounds = gameStr.Split(";");
            
            var redMin = 0;
            var blueMin = 0;
            var greenMin = 0;

            foreach (var round in rounds)
            {
                var redCount = 0;
                var blueCount = 0;
                var greenCount = 0;

                
                var hand = round.Substring(1).Split(", ");
                foreach (var handful in hand.Select(h => h.Trim()))
                {
                    var countStr = handful.Split(" ").First();
                    var colorStr = handful.Split(" ").Last();
                    
                    var count = int.Parse(countStr);
                    switch (colorStr)
                    {
                        case "red":
                            redCount += count;
                            if (redMin < redCount)
                            {
                                redMin = redCount;
                            }
                            break;
                        case "green":
                            greenCount += count;
                            if (greenMin < greenCount)
                            {
                                greenMin = greenCount;
                            }
                            break;
                        case "blue":
                            blueCount += count;
                            if (blueMin < blueCount)
                            {
                                blueMin = blueCount;
                            }
                            break;
                    }
                }

                if (redCount > red || blueCount > blue || greenCount > green)
                {
                    //possible = false;
                    //break;
                }
            }

            var power = redMin * blueMin * greenMin;
            total += power;

            if (possible)
            {
                //total += gameNumber;
            }
        }

        return total;
    }
}