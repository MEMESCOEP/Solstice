using Solstice.Common;

namespace Solstice.Audio.Utilities.Decoders;

public static class AudioDecoder
{
    public static readonly Dictionary<byte[], AudioFormat> DecoderMap = new()
    {
        { new byte[] { 0x52, 0x49, 0x46, 0x46 }, AudioFormat.Wave }, // RIFF
        { new byte[] { 0x66, 0x4C, 0x61, 0x43 }, AudioFormat.Flac }, // fLaCs
    };
    
    public static bool CreateDecoder(string path, out IAudioDecoder? decoder)
    {
        // We use a static factory method to create the appropriate decoder.
        // This method uses magic number detection to determine the file type.
        decoder = null;
        byte[] rawData;
        try
        {
            rawData = File.ReadAllBytes(path);
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Failed to read audio file: {path}. Error: {ex.Message}");
            return false; // Could not read the file
        }
        
        if (rawData.Length < 4)
        {
            Logger.Log(LogLevel.Error, $"Audio file {path} is too short to determine type.");
            return false; // Not enough data to determine file type
        }

        AudioFormat format = AudioFormat.Unknown;
        foreach (var kvp in DecoderMap)
        {
            if (rawData.Take(kvp.Key.Length).SequenceEqual(kvp.Key))
            {
                format = kvp.Value;
                break;
            }
        }
        
        switch (format)
        {
            case AudioFormat.Wave:
                decoder = new WaveDecoder(rawData);
                return true;
            case AudioFormat.Flac:
                decoder = new FlacDecoder(rawData);
                return true;
            case AudioFormat.Unknown:
                break; // Continue to log unsupported format
        }
        
        Logger.Log(LogLevel.Error, $"Unsupported audio format for file: {path}");

        return false;
    }
}