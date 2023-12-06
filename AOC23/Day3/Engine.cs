using System.Text;

namespace AOC23.Day3;

public class Engine
{
    public int SumSchematic(string input)
    {
        var schematic = new Schematic();
        var lines = input.Split("\n").Where(l => !string.IsNullOrEmpty(l));

        foreach (var line in lines)
        {
            schematic.AddLine(line);
        }

        return schematic.Calculate();
    }

    private class Schematic
    {
        private List<List<int>> _map = new();
        private List<Dictionary<int, int>> _parts = new();

        public int Calculate()
        {
            var total = 0;
            for (int i = 0; i < _map.Count; i++)
            {
                for(int x = 0; x < _map[i].Count; x++)
                {
                    if (_map[i][x] == -2)
                    {
                        var parts = new List<int>();
                        // check for part numbers above
                        if (i != 0)
                        {
                            var min = x == 0 ? x : x - 1;
                            var max = x + 1 >= _map[i].Count ? _map[i].Count : x + 1 + 1;
                            for (var a = min; a < max; a++)
                            {
                                if (_map[i - 1][a] > 0)
                                {
                                    // Get part number
                                    var num = _parts[i-1][_map[i-1][a]];
                                    var numLen = num.ToString().Length;
                                    var nexta = a;
                                    do
                                    {
                                        nexta++;
                                    } while(nexta < max && _map[i - 1][a] == _map[i - 1][nexta]);

                                    a = nexta - 1;
                                    parts.Add(num);
                                }
                            }
                        }

                        // Check for part either side of num
                        if (x != 0)
                        {
                            if (_map[i][x - 1] > 0)
                            {
                                // Get part number
                                var num = _parts[i][_map[i][x-1]];
                                parts.Add(num);
                            }
                        }

                        if (x + 1 < _map[i].Count)
                        {
                            if (_map[i][x + 1] > 0)
                            {
                                // Get part number
                                var num = _parts[i][_map[i][x+1]];
                                parts.Add(num);
                            }
                        }

                        // Check for part in line below
                        if (i + 1 < _map.Count)
                        {
                            var min = x == 0 ? x : x - 1;
                            var max = x + 1 >= _map[i].Count ? _map[i].Count : x + 1 + 1;
                            for (var a = min; a < max; a++)
                            {
                                if (_map[i + 1][a] > 0)
                                {
                                    // Get part number
                                    var num = _parts[i+1][_map[i+1][a]];
                                    var numLen = num.ToString().Length;
                                    var nexta = a;
                                    do
                                    {
                                        nexta++;
                                    } while(nexta < max && _map[i + 1][a] == _map[i + 1][nexta]);

                                    a = nexta - 1;
                                    parts.Add(num);
                                }
                            }
                        }
                        
                        if (parts.Count == 2)
                        {
                            total += parts[0] * parts[1];
                        }
                    }

                    /*if (_map[i][x] > 0)
                    {
                        // Get part number
                        var num = _parts[i][_map[i][x]];
                        var numLen = num.ToString().Length;
                        var isPart = false;

                        // Check for symbol in line above
                        if (i != 0)
                        {
                            var min = x == 0 ? x : x - 1;
                            var max = x + numLen >= _map[i].Count ? _map[i].Count : x + numLen + 1;
                            for (var a = min; a < max; a++)
                            {
                                if (_map[i - 1][a] == -1)
                                {
                                    total += num;
                                    isPart = true;
                                    break;
                                }
                            }

                            if (isPart)
                            {
                                x += numLen - 1;
                                continue;
                            }
                        }

                        // Check for part either side of num
                        if (x != 0)
                        {
                            if (_map[i][x - 1] == -1)
                            {
                                total += num;
                                x += numLen - 1;
                                continue;
                            }
                        }

                        if (x + numLen < _map[i].Count)
                        {
                            if (_map[i][x + numLen] == -1)
                            {
                                total += num;
                                x += numLen - 1;
                                continue;
                            }
                        }

                        // Check for part in line below
                        if (i + 1 < _map.Count)
                        {
                            var min = x == 0 ? x : x - 1;
                            var max = x + numLen >= _map[i].Count ? _map[i].Count : x + numLen + 1;
                            for (var a = min; a < max; a++)
                            {
                                if (_map[i + 1][a] == -1)
                                {
                                    total += num;
                                    isPart = true;
                                    break;
                                }
                            }

                            if (isPart)
                            {
                                x += numLen - 1;
                                continue;
                            }
                        }

                        x += numLen - 1;
                    }*/
                }
            }

            return total;
        }
        
        public void AddLine(string line)
        {
            var mapLine = new List<int>();
            var parts = new Dictionary<int, int>();
            var currentPart = 1;
            for (int i = 0; i < line.Length; i++)
            {
                if (int.TryParse(line[i].ToString(), out var dig))
                {
                    var wholeNum = GetCurrentNum(line.Substring(i));
                    i += wholeNum.Length - 1;
                    parts.Add(currentPart, int.Parse(wholeNum));
                    for (int y = 0; y < wholeNum.Length; y++)
                    {
                        mapLine.Add(currentPart);
                    }
                    currentPart++;
                }
                else if (line[i] == '.')
                {
                    mapLine.Add(0);
                }
                else if (line[i] == '*')
                {
                    mapLine.Add(-2);
                }
                else
                {
                    mapLine.Add(-1);
                }
            }
            
            _map.Add(mapLine);
            _parts.Add(parts);
        }

        private string GetCurrentNum(string input)
        {
            var sb = new StringBuilder();
            foreach (var ch in input)
            {
                if (int.TryParse(ch.ToString(), out var dig))
                {
                    sb.Append(dig);
                }
                else
                {
                    break;
                }
            }

            return sb.ToString();
        }
    }
}