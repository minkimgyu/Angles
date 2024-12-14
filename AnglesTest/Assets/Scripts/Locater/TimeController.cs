using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullTimeController : ITimeController
{
    public void Restart() { }
    public void Stop() { }
    public void ResetRatio(float ratio) { }
}

public interface ITimeController
{
    void Restart();
    void Stop();
    void ResetRatio(float ratio);
}

public class TimeController : ITimeController
{
    public void Restart()
    {
        Time.timeScale = 1.0f;
    }

    public void Stop()
    {
        Time.timeScale = 0.0f;
    }

    public void ResetRatio(float ratio)
    {
        Time.timeScale = ratio;
    }
}
