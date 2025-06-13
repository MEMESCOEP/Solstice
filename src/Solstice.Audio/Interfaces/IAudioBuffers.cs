using Solstice.Audio.Enums;

namespace Solstice.Audio.Interfaces;

public interface IAudioBuffers
{
    /// <summary>
    /// Once set to false, can not be set to true again.
    /// </summary>
    bool Enabled { get; set; }
    
    /// <summary>
    /// Mono or stereo audio channels. (1 or 2 channels)
    /// </summary>
    public AudioChannels Channels { get; }
    
    /// <summary>
    /// The sample rate of the audio data, typically 44100 Hz or 48000 Hz.
    /// If the audio is mono, the sample rate is equivalent to the number of samples per second.
    /// If the audio is stereo, the sample rate is equivalent to SampleRate * 2, as there are two channels of audio data. Where every 2 samples is a pair of left and right channel samples.
    /// </summary>
    public int SampleRate { get; }
    
    /// <summary>
    /// The buffer size in samples. This also indicates the delay in samples that the audio will have.
    /// If the audio is stereo, the buffer size will be multiplied by 2, as there are two channels of audio data.
    /// </summary>
    public int BufferSize { get; }
    
    /// <summary>
    /// Set a buffer of audio data to the queued buffer.
    /// This can be called multiple times, however it will overwrite the previous buffer.
    /// </summary>
    /// <param name="buffer">The audio data to add to the queue.</param>
    public void SetBuffer(Span<float> buffer);
    
    /// <summary>
    /// This event is triggered when the audio buffers are updated, and can receive more audio data.
    /// You should ONLY enqueue stuff through this event, as it is called on the audio thread.
    /// </summary>
    public Action<IAudioBuffers>? OnBuffersUpdated { get; set; }

    public void Close();
}