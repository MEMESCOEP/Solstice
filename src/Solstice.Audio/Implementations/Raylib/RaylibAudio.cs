using System.Numerics;
using Hexa.NET.Raylib;
using Solstice.Audio.Classes;
using rl = Hexa.NET.Raylib.Raylib;

using Solstice.Audio.Interfaces;
using Solstice.Audio.Utilities;

namespace Solstice.Audio.Implementations.Raylib;

public class RaylibAudio : IAudio
{
    public IChannel MasterChannel { get; set; }
    public List<IChannel> AudioChannels { get; }
    public IAudioBuffers AudioBuffers { get; }
    public AudioContext AudioContext { get; } = new AudioContext();

    public RaylibAudio(AudioSettings settings)
    {
        AudioMath.InitPrecachedBuffers();
        
        // Initialize Raylib audio system
        rl.InitAudioDevice();

        AudioBuffers = new RaylibAudioBuffers(settings);
        AudioChannels = new List<IChannel>();
        MasterChannel = new MasterChannel();
        
        AudioBuffers.UpdateBuffer += UpdateBuffer;
    }

    private void UpdateBuffer(IAudioBuffers obj)
    {
        int sampleRate   = AudioBuffers.SampleRate;
        int numChannels  = (int)AudioBuffers.Channels; // 1 or 2
        int bufferLength = AudioBuffers.BufferSize * numChannels;

        var totalBuffer = new float[bufferLength];
        AudioContext.Update(AudioBuffers.BufferSize / (float) AudioBuffers.SampleRate, AudioBuffers.BufferSize);

        foreach (var channel in AudioChannels)
        {
            var tempBuffer = new float[bufferLength];

            channel.Process(tempBuffer, sampleRate, numChannels, AudioContext);

            for (int i = 0; i < bufferLength; i++)
                totalBuffer[i] += channel.IsMuted ? 0f : tempBuffer[i];
        }

        MasterChannel.Process(totalBuffer, sampleRate, numChannels, AudioContext);

        for (int i = 0; i < bufferLength; i++)
        {
            // Clip audio values to [-1.0f, 1.0f]. 
            if (totalBuffer[i] >  1f) totalBuffer[i] =  1f;
            if (totalBuffer[i] < -1f) totalBuffer[i] = -1f;
        }

        AudioBuffers.SetBuffer(totalBuffer);
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
    }
}