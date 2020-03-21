using System;
using UnityEngine.SceneManagement;

public static class GlobalCountDown
{
    static DateTime TimeStarted;
    static TimeSpan TotalTime;

    public static void StartCountDown(TimeSpan totalTime)
    {
        TimeStarted = DateTime.UtcNow;
        TotalTime = totalTime;
    }

    public static TimeSpan TimeLeft
    {
        get
        {
            var result = TotalTime - (DateTime.UtcNow - TimeStarted);
            return result;
        }
    }
}