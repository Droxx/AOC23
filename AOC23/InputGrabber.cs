namespace AOC23;

public static class InputGrabber
{
    public static string GetInput(string exitStr = "")
    {
        Console.WriteLine("give input");
        string scInput = "";
        while (true)
        {
            var line = Console.ReadLine();
            if (line == exitStr)
            {
                break;
            }
            scInput += line + "\n";
        }

        return scInput;
    }
}