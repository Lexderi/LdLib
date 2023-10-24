using System.Diagnostics;

namespace LdLib;

public static class Time
{
    /// <summary>
    /// the delta from the last frame to this one
    /// </summary>
    public static float UpdateDelta { get; internal set; }
    
    /// <summary>
    /// Elapsed time since this program started
    /// </summary>
    public static TimeSpan SinceProgramStart =>
        DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime();
}