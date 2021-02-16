using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    /// <summary>
    /// Returns a given percentage of a value.
    /// </summary>
    /// <param name="percentage"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double Pct(double percentage, double value)
    {
        return Math.Round(value * percentage / 100);
    }
}
