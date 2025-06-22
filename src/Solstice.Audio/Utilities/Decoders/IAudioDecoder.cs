namespace Solstice.Audio.Utilities.Decoders;

public interface IAudioDecoder
{
    public int SampleRate { get; }
    public int Channels { get; }
    public int BitDepth { get; }
    
    /// <summary>
    /// Duration of the audio in samples.
    /// </summary>
    public UInt64 Duration { get; }

    public byte[] RawData { get; }

    public float[] Decode();
}