using System.Text.RegularExpressions;

namespace AOC23.Day6;

public class BoatRace
{
    private List<Race> _races = new List<Race>();
    
    public int WinOptionsMult(string input)
    {
        ParseInput(input);

        return 5;
    }

    private void ParseInput(string input)
    {
        var lines = input.Split("\n").Where(l => !string.IsNullOrEmpty(l)).ToList();
        var times = new List<int>();
        var distances = new List<int>();
        var regex = new Regex(@"\.*(\d+)");
         
        // Times
        var timeMatches= regex.Matches(lines[0]);
        for(var i = 0; i < timeMatches.Count; i++)
        {
            times.Add(int.Parse(timeMatches[i].Groups[1].Value));
        }
        
        // Distances
        var distanceMatches= regex.Matches(lines[1]);
        for(var i = 0; i < distanceMatches.Count; i++)
        {
            distances.Add(int.Parse(distanceMatches[i].Groups[1].Value));
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
        public int Time { get; set; }
        public int Distance { get; set; }
    }
}