using System.Numerics;
using Solstice.Audio.Classes;

namespace Solstice.Audio.Interfaces;

/// <summary>
/// IAudio is an interface to manage audio.
/// It runs on a separate thread, allowing it to process audio without blocking the main thread.
/// </summary>
public interface IAudio
{
    /// <summary>
    /// The master channel. This is the main channel that all audio is routed through.
    /// Changing the volume of this channel will affect all audio played through it.
    /// </summary>
    public IChannel MasterChannel { get; set; }
    
    /// <summary>
    /// A list of all audio channels. These channels can be used to play audio sources, and apply effects or filters to the audio.
    /// </summary>
    public List<IChannel> AudioChannels { get; }

    /// <summary>
    /// The raw audio buffers. It is not recommended to use directly, though if you need/want to, you can use it to access the raw audio data.
    /// Improper usage can lead to audio glitches/stuttering depending on performance.
    /// </summary>
    public IAudioBuffers AudioBuffers { get; }

    /// <summary>
    /// If the listener is position, this contains some data about the audio context, such as the listener's position and orientation.
    /// </summary>
    public AudioContext AudioContext { get; }
    
    /// <summary>
    /// After this method is called, the audio system stops processing audio.
    /// </summary>
    public void Close();
}