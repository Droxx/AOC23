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


        try
        {
            verInd = GetReflectionIndex(map.Cols,isClean);
        }
        catch (SmudgeException ex)
        {
            isClean = true;
            verInd = GetReflectionIndex(ex.RepairedLines, isClean);
        }

        try
        {
            horInd = GetReflectionIndex(map.Rows);
        }
        catch (SmudgeException ex)
        {
            isClean = true;
            horInd = GetReflectionIndex(ex.RepairedLines, isClean);
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

    private int GetReflectionIndex(List<string> list, bool isClean = false)
    {
        var currentReflectionSize = 0;
        var reflectionStartAfterIndex = -1;
        for(int i=0;i<list.Count-1;i++)
        {
            int currentReflection = -1;
            try
            {
                currentReflection = LengthOfReflection(list, i, isClean);
            }
            catch (RepairedLineException ex)
            {
                list[i] = ex.RepairedA;
                throw new SmudgeException(list);
            }

            if(currentReflection > currentReflectionSize)
            {
                currentReflectionSize = currentReflection;
                reflectionStartAfterIndex = i;
            }    
        }

        return reflectionStartAfterIndex;
    }

    private int LengthOfReflection(List<string> list, int startAfterIndex, bool isClean = false)
    {
        var a = startAfterIndex;
        var b = startAfterIndex + 1;
        while (a >= 0 && b < list.Count)
        {
            var compared = CompareLines(list[a], list[b], out var repairedA);
            if (compared == 0)
            {
                a--;
                b++;
            }
            else if (compared == 1 && !isClean)
            {
                throw new RepairedLineException(repairedA);
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

    private int CompareLines(string a, string b, out string? repairedA)
    {
        var repairedAArr = a.ToCharArray();
        repairedA = null;
        if (a == b)
        {
            return 0;
        }

        var diffs = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
                diffs++;
            if(diffs == 1)
            {
                repairedAArr[i] = b[i];
            }
        }

        repairedA = new string(repairedAArr);
        return diffs;
    }

    private class RepairedLineException : Exception
    {
        public string RepairedA { get; set; }
        public RepairedLineException(string repairedLine) : base("REPAIRED")
        {
            RepairedA = repairedLine;
        }
    }

    private class SmudgeException : Exception
    {
        public List<string> RepairedLines { get; set; }
        public SmudgeException(List<string> repairedLines) : base("REPAIRED")
        {
            RepairedLines = repairedLines;
        }
    }

    private class Map
    {
        public List<string> Rows { get; set; }
        public List<string> Cols { get; set; }
    }
}