using Hexa.NET.Raylib;
using rl = Hexa.NET.Raylib.Raylib;

using Solstice.Audio.Interfaces;

namespace Solstice.Audio.Implementations.Raylib;

public class RaylibAudio : IAudio
{
    public IChannel MasterChannel { get; set; }
    public List<IChannel> AudioChannels { get; }
    public List<IAudioSource> AudioSources { get; }
    public IAudioBuffers AudioBuffers { get; }

    public RaylibAudio(AudioSettings settings)
    {
        // Initialize Raylib audio system
        rl.InitAudioDevice();

        AudioBuffers = new RaylibAudioBuffers(settings);
        AudioChannels = new List<IChannel>();
        AudioSources = new List<IAudioSource>();
        MasterChannel = new MasterChannel();
    }
    
    public void Close()
    {
        // Stop and close the audio stream
        if (AudioBuffers is RaylibAudioBuffers raylibBuffers)
        {
            raylibBuffers.Enabled = false;
            raylibBuffers.Close();
        }

        // Close the audio device
        rl.CloseAudioDevice();

        // Clear the lists
        AudioChannels.Clear();
        AudioSources.Clear();
        
        MasterChannel = null;
    }
}