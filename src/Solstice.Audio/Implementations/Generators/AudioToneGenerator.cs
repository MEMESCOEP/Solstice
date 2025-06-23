using System.Numerics;
using Solstice.Audio.Classes;
using Solstice.Audio.Enums;
using Solstice.Audio.Interfaces;
using Solstice.Audio.Utilities;

namespace Solstice.Audio.Implementations.Generators;

public class AudioToneGenerator : IAudioSource
{
    public string Name { get; } = "Audio Tone Generator";
    public bool IsPositional { get; set; }
    public Vector3 Position { get; set; }

    public float Frequency;
    public float Amplitude;
    public WaveType WaveType { get; set; }

    private float _phaseIncrement;
    private float _phase;


    public AudioToneGenerator(WaveType waveType = WaveType.Sine,float frequency = 440f, float amplitude = 0.5f)
    {
        WaveType = waveType;
        Frequency = frequency;
        Amplitude = amplitude;
        _phase = 0f;
    }
    
    public void Process(Span<float> buffer, int sampleRate, int channels, AudioContext context)
    {
        _phaseIncrement = 2 * MathF.PI * Frequency / sampleRate;
        
        if (buffer.Length == 0 || sampleRate <= 0 || channels <= 0)
            return;

        if (channels == 1)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] += Amplitude * AudioMath.GenerateWaveSample(WaveType, _phase);
                _phase += _phaseIncrement;
                if (_phase >= 2*MathF.PI) _phase -= 2*MathF.PI;
            }
        }
        else
        {
            for (int i = 0; i < buffer.Length; i += 2)
            {
                float sample = Amplitude * AudioMath.GenerateWaveSample(WaveType, _phase);
                buffer[i] += sample; // left
                buffer[i + 1] += sample; // right
                _phase += _phaseIncrement;
                if (_phase >= 2*MathF.PI) _phase -= 2*MathF.PI;
            }
        }
        
    }
}