using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class Formatter
{
    private static SuffixData suffixData;

    public static string FormatValue(double value)
    {
        int mag = 0;
        double divisor;
        double shortNumber;
        string shortNumberText;
        string suffix = string.Empty;

        if (value > 999)
        {
            mag = (int)(Math.Floor(Math.Log10(value)) / 3);
            divisor = Math.Pow(10, mag * 3);
            shortNumber = value / divisor;
            shortNumberText = shortNumber.ToString("N2");
        }
        else
        {
            shortNumberText = value.ToString("N0");
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
}
