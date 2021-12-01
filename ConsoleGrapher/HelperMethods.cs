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
            lines
                .Select(l => l.Split(' '))
                .Select(e => new Reading(int.Parse(e[0]), double.Parse(e[1])))
        );

        return readings;
    }
}