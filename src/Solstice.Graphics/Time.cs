namespace Solstice.Graphics;

public class Time
{
    /// <summary>
    /// The time in seconds since the window was created.
    /// </summary>
    public float TotalTime { get; internal set; }

    /// <summary>
    /// The time in seconds since the last frame.
    /// </summary>
    public float DeltaTime { get; internal set; }

    /// <summary>
    /// The current frame number.
    /// </summary>
    public UInt64 FrameNumber { get; internal set; }

    public override string ToString()
    {
        return $"TotalTime: {TotalTime:F2}s, DeltaTime: {DeltaTime:F2}s, FrameNumber: {FrameNumber}";
    }
}