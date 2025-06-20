using NAudio;
using NAudio.Wave;

namespace Solstice.Audio.Utilities;

/// <summary>
/// A static helper class for loading audio files.
/// </summary>
public static class AudioLoaders
{
    public static float[] LoadFile(string path, bool forceStereo = false)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Audio file not found: {path}");
        }

        using var reader = new AudioFileReader(path);
        var sampleCount = (int)reader.Length / sizeof(float);
        var samples = new float[sampleCount];

        int readSamples = reader.Read(samples, 0, sampleCount);

        if (forceStereo && reader.WaveFormat.Channels == 1)
        {
            // Convert mono to stereo by duplicating the channel
            var stereoSamples = new float[readSamples * 2];
            for (int i = 0; i < readSamples; i++)
            {
                stereoSamples[i * 2] = samples[i];
                stereoSamples[i * 2 + 1] = samples[i];
            }
            return stereoSamples;
        }

        return samples.Take(readSamples).ToArray();
    }
}