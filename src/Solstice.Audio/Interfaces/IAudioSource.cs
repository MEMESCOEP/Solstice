namespace Solstice.Audio.Interfaces;

/// <summary>
/// An audio source generates sound data that can be played through audio channels.
/// </summary>
public interface IAudioSource
{
    public string Name { get; }
    
    public void GenerateSoundData(Span<float> buffer, int sampleRate, int channels);
}