namespace Solstice.Audio.Utilities.Decoders;

public class FlacDecoder : IAudioDecoder
{
    public int SampleRate { get; private set; }
    public int Channels { get; private set; }
    public int BitDepth { get; private set; }
    public ulong Duration { get; private set; }
    public byte[] RawData { get; private set; }

    public FlacDecoder(byte[] rawData)
    {
        this.RawData = rawData;
    }
    
    public float[] Decode()
    {
        // Placeholder for actual FLAC decoding logic
        // This should read the FLAC file and return the audio data as a float array
        throw new NotImplementedException("FLAC decoding is not implemented yet.");
    }
}