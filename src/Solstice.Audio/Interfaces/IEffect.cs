using Solstice.Audio.Enums;

namespace Solstice.Audio.Interfaces;

public interface IEffect
{
    public string Name { get; }
    public string Description { get; }
    public bool IsEnabled { get; set; }
    public float Mix { get; set; }

    public void ApplyEffect(Span<float> buffer, int sampleRate, AudioChannels channels);
    public void ApplyEffect(Span<short> buffer, int sampleRate, AudioChannels channels);
}