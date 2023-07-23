﻿namespace EfLight.Extensions;
internal static class StringExtensions
{
    /// <summary>
    /// Prints the string to the <see cref="Console"/>
    /// </summary>
    /// <param name="s"></param>
    public static void ToConsole(this string s)
    {
        Console.WriteLine(s);
    }
}
