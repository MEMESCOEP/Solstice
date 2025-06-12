using Solstice.Common;
using Solstice.Graphics;

var window = WindowFactory.CreateWindow(WindowSettings.Default() with
{
    Title = "Runtime",
});

window.OnUpdate += (w) =>
{
    Logger.Log(LogLevel.Info, $"Window updated: {(1.0f / w.Time.DeltaTime):F2} FPS, Frame: {w.Time.FrameNumber}, Total Time: {w.Time.TotalTime:F2}s");
};

window.Run();