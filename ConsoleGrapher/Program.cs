// See https://aka.ms/new-console-template for more information


using System;
using System.Collections.Generic;
using System.Linq;

ConsoleSetFullscreen();
Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

int CWIDTH = Console.BufferWidth;
int CHEIGHT = Console.BufferHeight;

List<Reading> reading = new();
reading.Add(new Reading(1,1));

GraphList(reading);

Console.ReadKey();

#region Stuff

void GraphList(List<Reading> list, bool allowExtremes = true)
{
    if (!allowExtremes)
    {
        
    }

    var maxKpS = list.Max(v => v.KpS);
    var vals = list.Count;

    for (int i = 0; i < 8; i++)
        Console.WriteLine();
    
    Console.WriteLine('^');
    for (int i = 0; i < CHEIGHT - 10; i++)
        Console.WriteLine('|');
    
    for (int i = 0; i < CWIDTH - 1; i++)
        Console.Write('-');

    Console.Write('>');
}

Reading ReadReading()
{
    Console.Clear();
    Console.WriteLine
    (@"
      _   _                 _____                _ _             
     | \ | |               |  __ \              | (_)            
     |  \| | _____      __ | |__) |___  __ _  __| |_ _ __   __ _ 
     | . ` |/ _ \ \ /\ / / |  _  // _ \/ _` |/ _` | | '_ \ / _` |
     | |\  |  __/\ V  V /  | | \ \  __/ (_| | (_| | | | | | (_| |
     |_| \_|\___| \_/\_/   |_|  \_\___|\__,_|\__,_|_|_| |_|\__, |
                                                            __/ |
                                                           |___/     
    ");

    Console.Write("\n\nKills: ");
    var kills = int.Parse(Console.ReadLine() ?? string.Empty);
    
    Console.Write("\n\nSeconds: ");
    var seconds = double.Parse(Console.ReadLine() ?? string.Empty);

    return new Reading(kills, seconds);
}

static int Map (int value, int fromSource, int toSource, int fromTarget, int toTarget)
{
    return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
}

void ConsoleSetFullscreen()
    => Console.SetWindowSize(Console.LargestWindowWidth - 10, Console.LargestWindowHeight - 10);

record Reading(int Kills, double Seconds)
{
    public double KpS => Kills / Seconds;
    public double KpM => Kills / (Seconds / 60);
};

#endregion