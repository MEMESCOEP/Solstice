using Solstice.Audio.Enums;

namespace Solstice.Audio.Interfaces;

public interface IEffect
{
    public string Name { get; }
    public string Description { get; }
    public bool IsEnabled { get; set; }
    public float Mix { get; set; }

    public void Process(Span<float> buffer, int sampleRate, int channels);
}