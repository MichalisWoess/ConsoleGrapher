using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class HelperMethods
{
    public static List<Reading> ReadingsFromFile(string fileName, bool ignoreFirstLine = false)
    {
        string[] lines = File.ReadAllLines(fileName);

        List<Reading> readings = new();
        readings.AddRange
        (
            (ignoreFirstLine ? lines.Skip(1) : lines)
                .Select(l => l.Split(' '))
                .Select(e => new Reading(int.Parse(e[0]), double.Parse(e[1])))
        );

        return readings;
    }

    public static void WriteColored(char c, ConsoleColor foreGroundColor, ConsoleColor backGroundColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = foreGroundColor;
        Console.BackgroundColor = backGroundColor;
        
        Console.Write(c);
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
    }
}