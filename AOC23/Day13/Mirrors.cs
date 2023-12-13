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

        verInd = GetReflectionIndex(map.Cols);
        horInd = GetReflectionIndex(map.Rows);
        
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

    private int GetReflectionIndex(List<string> list)
    {
        var currentReflectionSize = 0;
        var reflectionStartAfterIndex = -1;
        for(int i=0;i<list.Count-1;i++)
        {
            var currentReflection = LengthOfReflection(list, i);
            
            if(currentReflection > currentReflectionSize)
            {
                currentReflectionSize = currentReflection;
                reflectionStartAfterIndex = i;
            }    
        }

        return reflectionStartAfterIndex;
    }

    private int LengthOfReflection(List<string> list, int startAfterIndex)
    {
        var a = startAfterIndex;
        var b = startAfterIndex + 1;
        bool foundSmudge = false;
        while (a >= 0 && b < list.Count)
        {
            var compared = CompareLines(list[a], list[b]);
            if (compared == 0)
            {
                a--;
                b++;
            }
            else if (compared == 1)
            {
                a--;
                b++;
                foundSmudge = true;
            }
            else 
            {
                return 0;
                break;
            }
        }

        if(a == -1 && foundSmudge)
        {
            return 1;
        }
        if(b == list.Count && foundSmudge)
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

    private int CompareLines(string a, string b)
    {
        var repairedAArr = a.ToCharArray();
        if (a == b)
        {
            return 0;
        }

        var diffs = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if(diffs == 0)
            {
                repairedAArr[i] = b[i];
            }
            if (a[i] != b[i])
                diffs++;

        }

        return diffs;
    }

    private class Map
    {
        public List<string> Rows { get; set; }
        public List<string> Cols { get; set; }
    }
}