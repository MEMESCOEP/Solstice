using Solstice.Audio.Interfaces;

namespace Solstice.Audio.Implementations;

public class MasterChannel : IChannel
{
    public string Name { get; set; } = "Master Channel";
    public float Volume { get; set; } = 1.0f;
    public float Pan { get; set; } = 0.0f;
    public bool IsMuted { get; set; } = false;
}