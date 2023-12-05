// See https://aka.ms/new-console-template for more information

using AOC23.Day1;

Console.WriteLine("AOC 23 - BEGIN");
Console.WriteLine("Which day?");
var dayStr = Console.ReadLine();

if (int.TryParse(dayStr, out var day))
{
    switch (day)
    {
        case 1:
            Console.WriteLine("Please enter calibration input, followed by an empty line to compute");
            string calibInput = "";
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "")
                {
                    break;
                }
                calibInput += line + "\n";
            }
            var calibration = new CalibrationValues(calibInput);
            Console.WriteLine(calibration.Compute());
            break;
        default:
            Console.WriteLine("Day not implemented");
            break;
    }
}
else
{
    Console.WriteLine("Invalid day");
}

Console.WriteLine("Press any key to exit...");