namespace AOC23.Day5;

public class Map
{
    private Dictionary<long, long> _map = new();

    public void AddMapping(long destStart, long sourceStart, long range)
    {
        long count = 0;
        for(long i = sourceStart; i < sourceStart + range; i++)
        {
            _map.Add(i, destStart + count);
            count++;
        }
    }
    
    public long GetDest(long source)
    {
        if (_map.TryGetValue(source, out var dest))
        {
            return dest;
        }

        return source;  
    }
}