namespace AOC23.Day5;

public class Map
{
    private List<Range> _ranges = new();

    public void AddMapping(long destStart, long sourceStart, long range)
    {
        _ranges.Add(new Range(sourceStart, sourceStart + range, destStart, destStart + range));
    }
    
    public long GetDest(long source)
    {
        var validRange = _ranges.FirstOrDefault(r => r.IsInRange(source));
        if (validRange != null)
        {
            return validRange.GetDest(source);
        }
        else
        {
            return source;
        }
    }

    private class Range
    {
        private long SrcMin { get; set; }
        private long SrcMax { get; set; }
        
        private long DstMin { get; set; }
        private long DstMax { get; set; }
        
        public Range(long srcMin, long srcMax, long dstMin, long dstMax)
        {
            SrcMin = srcMin;
            SrcMax = srcMax;
            DstMin = dstMin;
            DstMax = dstMax;
        }
        
        public bool IsInRange(long num)
        {
            return num >= SrcMin && num <= SrcMax;
        }
        
        public long GetDest(long num)
        {
            if (IsInRange(num))
            {
                return num - SrcMin + DstMin;
            }

            return num;
        }
    }
}