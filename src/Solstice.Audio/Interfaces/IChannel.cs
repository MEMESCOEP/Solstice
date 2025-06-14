namespace Solstice.Audio.Interfaces;

/// <summary>
/// A channel represents an audio channel which can hold custom audio data, effects, or other audio-related information.
/// Sources are played on channels, and channels route and mix the audio data to the output.
/// </summary>
public interface IChannel
{
    public string Name { get; set; }
    
    /// <summary>
    /// Volume multiplier the channel. [0.0f, 1.0f] range.
    /// </summary>
    public float Volume { get; set; }
    
    /// <summary>
    /// Pan multiplier for the channel. [-1.0f, 1.0f] range (Left to Right).
    /// </summary>
    public float Pan { get; set; }
    
    public bool IsMuted { get; set; }
    
    public List<IEffect> Effects { get; }
     
    public List<IAudioSource> Sources { get; }
    
    /// <summary>
    /// Modify the audio data in the buffer, based on the channel's properties and effects.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="sampleRate"></param>
    /// <param name="channels"></param>
    public void Process(Span<float> buffer, int sampleRate, int channels);
}