// See https://aka.ms/new-console-template for more information

using AOC23.Day1;
using AOC23.Day2;
using AOC23.Day3;
using AOC23.Day4;

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
        case 2:
            Console.WriteLine("Please enter game input, followed by an empty line to continue");
            string gameInput = "";
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "")
                {
                    break;
                }
                gameInput += line + "\n";
            }
            Console.WriteLine("Please enter game red max");
            var red = Console.ReadLine();
            Console.WriteLine("Please enter game green max");
            var green = Console.ReadLine();
            Console.WriteLine("Please enter game blue max");
            var blue = Console.ReadLine();
            var gameOb = new CubeGame(gameInput);
            Console.WriteLine(gameOb.Play(int.Parse(red), int.Parse(blue), int.Parse(green)));
            break;
        case 3:
            Console.WriteLine("Please enter engine input, followed by an empty line to continue");
            string engInput = "";
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "")
                {
                    break;
                }
                engInput += line + "\n";
            }

            var en = new Engine();
            Console.WriteLine(en.SumSchematic(engInput));
            break;
        case 4:
            Console.WriteLine("Give input");
            string scInput = "";
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "")
                {
                    break;
                }
                scInput += line + "\n";
            }
            var sc = new Scratchcards();
            Console.WriteLine(sc.CalculateWinnings(scInput));
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