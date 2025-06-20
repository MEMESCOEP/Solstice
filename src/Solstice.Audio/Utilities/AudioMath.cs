using Solstice.Audio.Enums;

namespace Solstice.Audio.Utilities;

public static class AudioMath
{
    public static float[] PanSample(float[] input, float pan)
    {
        if (input.Length != 2)
            throw new ArgumentException("Input sample must be a stereo sample with exactly 2 channels.");
        
        pan = Math.Clamp(pan, -1.0f, 1.0f);
        float left = input[0] * (1.0f - pan);
        float right = input[1] * (1.0f + pan);
        left = Math.Clamp(left, -1.0f, 1.0f);
        right = Math.Clamp(right, -1.0f, 1.0f);
        return [left, right];
    }

    private static float[] _noiseBuffer = new float[1024 * 1024]; // Buffer for noise generation
    public static void InitPrecachedBuffers()
    {
        Random random = new Random();
        for (int i = 0; i < _noiseBuffer.Length; i++)
        {
            _noiseBuffer[i] = (float)(random.NextDouble() * 2.0 - 1.0); // Fill with random values between -1 and 1
        }
    }

    public static float GenerateWaveSample(WaveType waveType, float phase)
    {
        switch (waveType)
        {
            case WaveType.Sine:
                return MathF.Sin(phase);
            case WaveType.Square:
                return MathF.Sign(MathF.Sin(phase));
            case WaveType.Saw:
                return 2.0f * (phase / (2 * MathF.PI)) - 1.0f;
            case WaveType.Triangle:
                return 2.0f * MathF.Abs(2.0f * (phase / (2 * MathF.PI)) - 1.0f) - 1.0f;
            case WaveType.Noise:
                int index = (int)(phase / (2 * MathF.PI) * _noiseBuffer.Length) % _noiseBuffer.Length;
                return _noiseBuffer[index];
            default:
                throw new ArgumentOutOfRangeException(nameof(waveType), waveType, null);
        }
    }

    public static float FrequencyFromNote(Note note, int octave, float A4 = 440f)
    {
        if (octave < 0 || octave > 8)
            throw new ArgumentOutOfRangeException(nameof(octave), "Octave must be between 0 and 8.");

        int noteIndex = (int)note + (octave * 12);
        return A4 * MathF.Pow(2.0f, (noteIndex - 57) / 12.0f); // 57 is the index for A4
    }
}