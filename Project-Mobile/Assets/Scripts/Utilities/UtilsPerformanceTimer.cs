using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class UtilsPerformanceTimer
{
    private static Stopwatch stopwatch = new Stopwatch();

    private static void Start()
    {
        stopwatch.Reset();
        stopwatch.Start();
    }

    private static void Stop(string operation)
    {
        stopwatch.Stop();
        UnityEngine.Debug.Log($"{operation} time in ms: {stopwatch.ElapsedMilliseconds}");
    }
}
