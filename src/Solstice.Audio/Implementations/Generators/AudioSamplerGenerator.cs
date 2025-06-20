using Solstice.Audio.Interfaces;
using Solstice.Audio.Utilities;

namespace Solstice.Audio.Implementations.Generators;

public class AudioSamplerGenerator : IAudioSource
{
    public string Name { get; set; } = "Audio Sampler Generator";

    public bool IsLooping { get; set; }
    public bool IsPlaying { get; set; }
    public float Volume { get; set; } = 1.0f;
    public float Pan { get; set; } = 0.0f;
    public float PlaybackSpeed { get; set; } = 1.0f;
    
    private float[] _samples;
    private int _currentSampleIndex;
    private int _channels;

    /// <summary>
    /// Creates a new AudioSamplerGenerator with the provided samples. You can load samples from disk using AudioLoader.
    /// </summary>
    /// <param name="samples"></param>
    /// <param name="looping"></param>
    /// <param name="isPlaying"></param>
    /// <param name="channels">The number of channels in the sample. This might differ from the playback's channels!</param>
    /// <param name="amplitude"></param>
    /// <exception cref="ArgumentException"></exception>
    public AudioSamplerGenerator(float[] samples,bool looping = false, bool isPlaying = false, int channels = 2, float amplitude = 1.0f)
    {
        _samples = samples;
        IsLooping = looping;
        IsPlaying = isPlaying;
        _channels = channels;
        Volume = amplitude;

        if (_samples.Length % _channels != 0)
            throw new ArgumentException("Sample length must be a multiple of the number of channels.");
        
        _currentSampleIndex = 0;
    }
    
    public void Process(Span<float> buffer, int sampleRate, int channels)
    {
        if (!IsPlaying || _samples.Length == 0 || sampleRate <= 0 || channels <= 0)
        {
            buffer.Clear();
            return;
        }
        
        int samplesToProcess = buffer.Length / _channels;
        for (int i = 0; i < samplesToProcess; i++)
        {
            for (int c = 0; c < _channels; c++)
            {
                if (_currentSampleIndex >= _samples.Length)
                {
                    if (IsLooping)
                    {
                        _currentSampleIndex = 0;
                    }
                    else
                    {
                        buffer[i * _channels + c] = 0.0f;
                        continue;
                    }
                }

                buffer[i * _channels + c] = _samples[_currentSampleIndex] * Volume;
                _currentSampleIndex++;
            }
        }
        
        if (_currentSampleIndex >= _samples.Length && !IsLooping)
        {
            IsPlaying = false; // Stop playback if not looping
            _currentSampleIndex = 0; // Reset index
        }
        
        // Apply pan if stereo
        if (channels == 2 && _channels == 2)
        {
            for (int i = 0; i < buffer.Length; i += 2)
            {
                var panned = AudioMath.PanSample([buffer[i], buffer[i + 1]], Pan);
                buffer[i] = panned[0];
                buffer[i + 1] = panned[1];
            }
        }
        else if (channels == 1 && _channels == 2) // Mono playback
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (buffer[i * 2] + buffer[i * 2 + 1]) / 2 * Volume;
            }
        }
        
        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] > 1.0f) buffer[i] = 1.0f;
            else if (buffer[i] < -1.0f) buffer[i] = -1.0f;
        }
    }

}