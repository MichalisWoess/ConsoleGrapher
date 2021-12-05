using System;
using static System.Console;
using static System.ConsoleColor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HelperMethods;
using Microsoft.Win32.SafeHandles;

#region Setup
ConsoleSetFullscreen();
CursorVisible = false;
SetBufferSize(WindowWidth, WindowHeight);
OutputEncoding = Encoding.Unicode;
#endregion

int CWIDTH = BufferWidth;
int CHEIGHT = BufferHeight;

List<Reading> readings = ReadingsFromFile("Readings.txt", true);
List<(int Left, int Top)> valuePositions = new();

GraphList(readings, false, false);

ReadKey();

#region Stuff

void GraphList(List<Reading> list, bool allowExtremes = true, bool centered = true)
{
    if (!allowExtremes)
    {
        
    }

    var maxKpS = list.Max(v => v.KpS);
    var maxKills = list.Max(v => v.Kills);
    var valCount = list.Count;

    const int padding = 8;
    const int internalPaddingPercent = 0;
    var origin = PrintEmptyGraph(padding);
    
    var (left, top) = origin;
    origin.Left += 1;
    origin.Top -= 2;

    var availHeight = CHEIGHT - 2 - 2 * padding;
    var availWidth = CWIDTH - 1 - 2 * padding;

    int heightPadding = (int)(availHeight * (internalPaddingPercent / (double) 100));
    int paddedHeight = availHeight - (2 * heightPadding);
    
    int widthPadding = (int)(availWidth * (internalPaddingPercent / (double) 100));
    int paddedWidth = availWidth - (2 * widthPadding);

    origin.Left += widthPadding;
    origin.Top -= heightPadding;
    
    var listCount = centered ? valCount + 1 : valCount - 1;
    var spacing = paddedWidth / listCount;
    for (int i = 0; i < valCount; i++)
    {
        var mappedVal = Map(list[i].Kills, maxKills, paddedHeight);
        var offset = centered ? (i + 1) * spacing : i * spacing;
        PrintVal(origin, offset, (int)mappedVal, '=');
    }

    for (int i = 0; i < valuePositions.Count - 1; i++)
    {
        var dx = valuePositions[i + 1].Left - valuePositions[i].Left;
        var dy = valuePositions[i].Top - valuePositions[i + 1].Top;
        double slope = (double)dy / dx;

        var firstValueOrigin = valuePositions[i];
        firstValueOrigin.Left++;
        double currentVal = firstValueOrigin.Top;
        for (int j = 0; j < dx - 1; j++)
        {
            currentVal -= slope;
            SetCursorPosition
            (origin.Left + firstValueOrigin.Left + j - 9,
             origin.Top - (origin.Top - (int)Math.Round(currentVal))
            );
            
            Write(".");
        }
    }
}

void PrintVal((int Left, int Top) origin, int leftOffs, int val, char c)
{
    int x = origin.Left + leftOffs;
    int y = origin.Top - val;
    
    valuePositions.Add((x, y));
    SetCursorPosition(x, y);
    WriteColored(c, Red, Cyan);
}

(int Left, int Top) PrintEmptyGraph(int padding = 10)
{
    Write(new String('\n', padding));
    
    Write(new String(' ', padding));
    WriteLine("^");
    for (int i = 0; i < CHEIGHT - 2 - 2 * padding; i++)
    {
        Write(new String(' ', padding));
        WriteLine('|');
    }

    Write(new String(' ', padding));

    var pos = GetCursorPosition();
    Write(new String('-', CWIDTH - 1 - 2 * padding));

    WriteLine('>');
    Write(new String('\n', padding));

    return pos;
}

Reading ReadReading()
{
    Clear();
    WriteLine
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

    Write("\n\nKills: ");
    var kills = int.Parse(ReadLine() ?? string.Empty);
    
    Write("\n\nSeconds: ");
    var seconds = double.Parse(ReadLine() ?? string.Empty);

    return new Reading(kills, seconds);
}

static double Map (int value, int fromMax, int toMax, int fromMin = 0, int toMin = 0)
{
    return ((double)value / (fromMax - fromMin)) * ((double)toMax - toMin);
}

void ConsoleSetFullscreen()
    => SetWindowSize(LargestWindowWidth - 10, LargestWindowHeight - 10);


public record Reading(int Kills, double Seconds)
{
    public double KpS => Kills / Seconds;
    public double KpM => Kills / (Seconds / 60);
};

#endregion