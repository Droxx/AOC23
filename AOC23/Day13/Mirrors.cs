namespace AOC23.Day13;

public class Mirrors
{
    private List<Map> _maps = new List<Map>();
    public long Calculate(string input)
    {
        ParseInput(input);
        long result = 0;
        foreach (var map in _maps)
        {
            result += FindReflections(map);
        }
        return result;
    }

    private long FindReflections(Map map)
    {
        var verRowsAbove = 0;
        var horColsLeft = 0;
        var verInd = -1;
        var horInd = -1;
        var isClean = false;

        var uncleanVerInd = -1;
        var uncleanHorInd = -1;

        try
        {
            uncleanVerInd = GetReflectionIndex(map, true);
            uncleanHorInd = GetReflectionIndex(map, false);
            
            verInd = GetReflectionIndex(map, true, isClean);
            horInd = GetReflectionIndex(map, false, isClean);

        }
        catch (SmudgeException ex)
        {
            isClean = true;
            verInd = GetReflectionIndex(ex.RepairedMap, true, isClean);
            horInd = GetReflectionIndex(ex.RepairedMap, false, isClean);

            if (uncleanHorInd == horInd)
            {
                horInd = -1;
            }

            if (uncleanVerInd == verInd)
            {
                verInd = -1;
            }
        }

        //verRowsLeft = Math.Abs(map.Cols.Count - (map.Cols.Count - verInd));
        //horColsAbove = Math.Abs(map.Rows.Count - (map.Rows.Count -horInd));

        if (verInd >= 0)
        {
            horColsLeft = verInd + 1;
        }

        if (horInd >= 0)
        {
            verRowsAbove = horInd + 1;
        }
        
        if(verRowsAbove > 0 && horColsLeft > 0)
        {
            throw new Exception();
        }

        if (verRowsAbove == 0 && horColsLeft == 0)
        {
            throw new Exception();
        }

        if (verRowsAbove > 0)
        {
            return 100 * verRowsAbove;
        }

        return horColsLeft;
    }

    private int GetReflectionIndex(Map map, bool useCols, bool isClean = false)
    {
        var currentReflectionSize = 0;
        var reflectionStartAfterIndex = -1;
        var list = useCols ? map.Cols : map.Rows;
        for(int i=0;i<list.Count-1;i++)
        {
            int currentReflection = -1;
            /*try
            {*/
                currentReflection = LengthOfReflection(map, useCols, i, isClean);
            /*}
            catch (RepairedLineException ex)
            {
                list[ex.LineIndex] = ex.RepairedA;
                throw new SmudgeException(map);
            }*/

            if(currentReflection > currentReflectionSize)
            {
                currentReflectionSize = currentReflection;
                reflectionStartAfterIndex = i;
            }    
        }

        return reflectionStartAfterIndex;
    }

    private int LengthOfReflection(Map map, bool useCols, int startAfterIndex, bool isClean = false)
    {
        var a = startAfterIndex;
        var b = startAfterIndex + 1;
        var list = useCols ? map.Cols : map.Rows;
        while (a >= 0 && b < list.Count)
        {
            var compared = CompareLines(map, useCols, a, list[a], list[b], isClean);
            if (compared == 0)
            {
                a--;
                b++;
            }
            else
            {
                return 0;
                break;
            }
        }

        if(a == -1)
        {
            return 1;
        }
        if(b == list.Count)
        {
            return list.Count - 1;
        }
        

        return 0;
    }
    
    private void ParseInput(string input)
    {
        var blocks = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        foreach (var block in blocks)
        {
            var map = new Map();
            map.Rows = block.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            map.Cols = new List<string>();
            for(int i = 0; i < map.Rows[0].Length; i++)
            {
                var col = "";
                for(int j = 0; j < map.Rows.Count; j++)
                {
                    col += map.Rows[j][i];
                }
                map.Cols.Add(col);
            }
            _maps.Add(map);
        }
    }

    private int CompareLines(Map map, bool useCols, int index, string a, string b, bool isClean)
    {
        var repairedAArr = a.ToCharArray();
        int repairedAIndex = -1;
        char repairedAChar = ' ';
        string? repairedA = null;
        if (a == b)
        {
            return 0;
        }

        var diffs = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if(diffs == 0)
            {
                repairedAIndex = i;
                repairedAChar = b[i];
                repairedAArr[i] = b[i];
            }
            if (a[i] != b[i])
                diffs++;

        }

        repairedA = new string(repairedAArr);

        if (diffs == 1 && !isClean)
        {
            var list = useCols ? map.Cols : map.Rows;
            list[index] = repairedA;
            var otherList = useCols ? map.Rows : map.Cols;
            var repaired = otherList[repairedAIndex].ToCharArray();
            repaired[index] = repairedAChar;
            otherList[repairedAIndex] = new string(repaired);

            throw new SmudgeException(map);
        }
        
        return diffs;
    }

    private class RepairedLineException : Exception
    {
        public string RepairedA { get; set; }
        public int LineIndex { get; set; }
        public RepairedLineException(string repairedLine, int lineIndex) : base("REPAIRED")
        {
            LineIndex = lineIndex;
            RepairedA = repairedLine;
        }
    }

    private class SmudgeException : Exception
    {
        public Map RepairedMap { get; set; }
        public SmudgeException(Map repairedMap) : base("REPAIRED")
        {
            RepairedMap = repairedMap;
        }
    }

    private class Map
    {
        public List<string> Rows { get; set; }
        public List<string> Cols { get; set; }
    }
}