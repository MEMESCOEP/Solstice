using Solstice.Audio.Utilities.Decoders;

namespace Solstice.Audio.Utilities;

/// <summary>
/// A static helper class for loading audio files.
/// </summary>
public static class AudioLoaders
{
    /// <summary>
    /// Returns a 44100Hz, 16-bit stereo (2 channel) audio buffer from the specified file path.
    /// If the file is not in that format, it will be converted.
    /// </summary>
    public static float[] LoadFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Audio file not found: {path}");
        }

        if (AudioDecoder.CreateDecoder(path, out var decoder))
        {
            var audioData = decoder!.Decode();
            
            if (decoder.Channels == 1)
            {
                // Convert mono to stereo by duplicating the channel
                audioData = AudioConverter.MonoToStereo(audioData);
            }

            if (decoder.SampleRate == 44100 && decoder.BitDepth == 16)
            {
                return audioData;
            }

            if (decoder.SampleRate != 44100 && decoder.BitDepth == 16)
            { 
                return AudioConverter.Resample(audioData, decoder.SampleRate, 44100, 2);
            }
            
            throw new InvalidOperationException($"Unsupported audio format in file: {path}. Expected 44100Hz, 16-bit stereo. Got {decoder.SampleRate}Hz, {decoder.BitDepth}-bit, {decoder.Channels} channels.");
        }
        
        throw new InvalidOperationException($"Failed to create audio decoder for file: {path}");
    }
}