namespace Solstice.Audio.Utilities.Decoders;

public static class AudioConverter
{
    public static float[] Resample(float[] samples, int originalSampleRate, int targetSampleRate, int channels) 
    {
        if (samples == null || samples.Length == 0)
            throw new ArgumentException("Samples cannot be null or empty.");

        if (originalSampleRate <= 0 || targetSampleRate <= 0)
            throw new ArgumentException("Sample rates must be greater than zero.");

        if (channels <= 0)
            throw new ArgumentException("Number of channels must be greater than zero.");
        
        // Calculate the resampling ratio
        float ratio = (float)targetSampleRate / originalSampleRate;
        int newSampleCount = (int)(samples.Length * ratio);
        if (newSampleCount <= 0)
            throw new ArgumentException("Resampled sample count must be greater than zero.");
        float[] resampled = new float[newSampleCount * channels];
        for (int i = 0; i < newSampleCount; i++)
        {
            // Calculate the index in the original samples
            float originalIndex = i / ratio;
            int originalIndexInt = (int)originalIndex;
            float fractionalIndex = originalIndex - originalIndexInt;

            // Handle boundary conditions
            if (originalIndexInt >= samples.Length / channels)
                break;

            for (int c = 0; c < channels; c++)
            {
                // Get the sample from the original array
                float sampleValue = samples[originalIndexInt * channels + c];

                // If not the last sample, interpolate with the next sample
                if (originalIndexInt + 1 < samples.Length / channels)
                {
                    float nextSampleValue = samples[(originalIndexInt + 1) * channels + c];
                    sampleValue += fractionalIndex * (nextSampleValue - sampleValue);
                }

                resampled[i * channels + c] = sampleValue;
            }
        }

        return resampled;
    }
}