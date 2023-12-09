// See https://aka.ms/new-console-template for more information

using AOC23;
using AOC23.Day1;
using AOC23.Day2;
using AOC23.Day3;
using AOC23.Day4;
using AOC23.Day5;
using AOC23.Day6;
using AOC23.Day7;
using AOC23.Day8;
using AOC23.Day9;

Console.WriteLine("AOC 23 - BEGIN");
Console.WriteLine("Which day?");
var dayStr = Console.ReadLine();

if (int.TryParse(dayStr, out var day))
{
    switch (day)
    {
        case 1:
            var calibration = new CalibrationValues(InputGrabber.GetInput());
            Console.WriteLine(calibration.Compute());
            break;
        case 2:
            string gameInput = InputGrabber.GetInput();
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
            var en = new Engine();
            Console.WriteLine(en.SumSchematic(InputGrabber.GetInput()));
            break;
        case 4:
            var sc = new Scratchcards();
            Console.WriteLine(sc.CalculateWinnings(InputGrabber.GetInput()));
            break;
        case 5:
            var seedMapper = new SeedMapper();
            Console.WriteLine(seedMapper.GetClosestSeed(InputGrabber.GetInput("x")));
            break;
        case 6:
            var boatRate = new BoatRace();
            Console.WriteLine(boatRate.WinOptionsMult(InputGrabber.GetInput()));
            break;
        case 7:
            var camel = new CamelCards();
            Console.WriteLine(camel.Calculate(InputGrabber.GetInput()));
            break;
        case 8:
            var desertMap = new DesertMap();
            Console.WriteLine(desertMap.Navigate(InputGrabber.GetInput("/")));
            break;
        case 9:
            var oasis = new Oasis();
            Console.WriteLine(oasis.Calculate(InputGrabber.GetInput()));
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