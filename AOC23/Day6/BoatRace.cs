using System.Text.RegularExpressions;

namespace AOC23.Day6;

public class BoatRace
{
    private List<Race> _races = new List<Race>();
    
    public long WinOptionsMult(string input)
    {
        ParseInput(input);

        long winMult = 0;
        foreach (var race in _races)
        {
            winMult = winMult == 0 ? race.GetWinTimes() : race.GetWinTimes() * winMult;
        }

        return winMult;
    }

    private void ParseInput(string input)
    {
        var lines = input.Split("\n").Where(l => !string.IsNullOrEmpty(l)).ToList();
        var times = new List<long>();
        var distances = new List<long>();
        var regex = new Regex(@"\.*(\d+)");
         
        // Times
        var timeMatches= regex.Matches(lines[0]);
        for(var i = 0; i < timeMatches.Count; i++)
        {
            times.Add(long.Parse(timeMatches[i].Groups[1].Value));
        }
        
        // Distances
        var distanceMatches= regex.Matches(lines[1]);
        for(var i = 0; i < distanceMatches.Count; i++)
        {
            distances.Add(long.Parse(distanceMatches[i].Groups[1].Value));
        }

        if (times.Count != distances.Count)
        {
            throw new Exception("aah");
        }
        
        for (var i = 0; i < times.Count; i++)
        {
            _races.Add(new Race
            {
                Distance = distances[i],
                Time = times[i]
            });
        }
    }
    

    private class Race
    {
        public long Time { get; set; }
        public long Distance { get; set; }

        public long GetWinTimes()
        {
            var winCounter = 0;
            for(long i = 0; i <= Time; i++)
            {
                var speed = i;
                var remainingTime = Time - i;

                var totalDistance = speed * remainingTime;
                
                if(totalDistance > Distance)
                {
                    winCounter++;
                }
            }

            return winCounter;
        }
    }
}