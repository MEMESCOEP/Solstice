using Solstice.Audio.Enums;
using Solstice.Audio.Interfaces;

namespace Solstice.Audio;

public record AudioSettings(AudioBackend Backend, AudioChannels Channels, int SampleRate, int BufferSize)
{
    public static AudioSettings Default => new(AudioBackend.Raylib, AudioChannels.Stereo, 44100, 1024);
}

public static class AudioFactory
{
    public static IAudio CreateAudio(AudioSettings settings)
    {
        return settings.Backend switch
        {
            AudioBackend.Raylib => new Implementations.Raylib.RaylibAudio(settings),
            _ => throw new NotSupportedException($"Audio backend '{settings.Backend}' is not supported.")
        };
    }
}