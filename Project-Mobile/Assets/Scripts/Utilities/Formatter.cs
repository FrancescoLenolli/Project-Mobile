using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class Formatter
{
    private static SuffixData suffixData;
    private static int mag;
    private static double divisor;
    private static double shortNumber;
    private static string shortNumberText;
    private static string suffix;

    public static string FormatValue(double value)
    {
        mag = 0;
        divisor = 0;
        shortNumber = 0;
        shortNumberText = string.Empty;
        suffix = string.Empty;

        if (value > 999)
        {
            mag = (int)(Math.Floor(Math.Log10(value)) / 3);
            divisor = Math.Pow(10, mag * 3);
            shortNumber = value / divisor;
            shortNumberText = shortNumber.ToString("N2");
        }
        else
        {
            shortNumberText = value.ToString("N1");
        }

        int index = mag;
        if (index < GetSuffixes().Count)
            suffix = GetSuffixes()[index];

        return shortNumberText + suffix;
    }

    private static List<string> GetSuffixes()
    {
        if(suffixData == null)
        {
            suffixData = Resources.LoadAll<SuffixData>("Suffixes").ToList()[0];
        }

        return suffixData.suffixes;
    }

    public static string FormatTime(int seconds)
    {
        return $"{ TimeSpan.FromSeconds(seconds):hh\\:mm\\:ss}";
    }

    public static string FormatTime(double seconds)
    {
        return $"{ TimeSpan.FromSeconds(seconds):hh\\:mm\\:ss}";
    }

    public static string FormatTime(TimeSpan timeSpan)
    {
        return $"{ timeSpan:hh\\:mm\\:ss}";
    }
}
