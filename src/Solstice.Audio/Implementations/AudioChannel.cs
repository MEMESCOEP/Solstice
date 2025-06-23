using Solstice.Audio.Classes;
using Solstice.Audio.Interfaces;
using Solstice.Audio.Utilities;

namespace Solstice.Audio.Implementations;

public class AudioChannel : IChannel
{
    public string Name { get; set; } = "Audio Channel";
    
    public float Volume { get; set; } = 1.0f;
    
    public float Pan { get; set; } = 0.0f;
    
    public bool IsMuted { get; set; } = false;
    
    public List<IEffect> Effects { get; } = new List<IEffect>();
    
    public List<IAudioSource> Sources { get; } = new List<IAudioSource>();
    
    public void Process(Span<float> buffer, int sampleRate, int channels, AudioContext context)
    {
        buffer.Clear();
        
        if (IsMuted)
        {
            return;
        }
        
        foreach (var source in Sources)
            source.Process(buffer, sampleRate, channels, context);

        foreach (var effect in Effects)
            effect.Process(buffer, sampleRate, channels);
        
        for (int i = 0; i < buffer.Length; i++)
            buffer[i] *= Volume;

        if (channels == 2)
        {
            for (int i = 0; i < buffer.Length; i+=2)
            {
                var panned = AudioMath.PanSample([buffer[i], buffer[i + 1]], Pan);
                buffer[i] = panned[0];
                buffer[i + 1] = panned[1];
            }
        }
    }
}