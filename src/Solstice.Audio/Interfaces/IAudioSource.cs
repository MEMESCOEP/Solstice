using System.Numerics;
using Solstice.Audio.Classes;

namespace Solstice.Audio.Interfaces;

/// <summary>
/// An audio source generates sound data that can be played through audio channels.
/// </summary>
public interface IAudioSource
{
    public string Name { get; }
    public bool IsPositional { get; set; }
    public Vector3 Position { get; set; }


    public void Process(Span<float> buffer, int sampleRate, int channels, AudioContext context);
}