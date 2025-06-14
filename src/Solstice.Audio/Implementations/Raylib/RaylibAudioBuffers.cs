using Solstice.Audio.Enums;
using Solstice.Audio.Interfaces;

using Hexa.NET.Raylib;
using rl = Hexa.NET.Raylib.Raylib;

namespace Solstice.Audio.Implementations.Raylib;

public class RaylibAudioBuffers : IAudioBuffers
{
    public bool Enabled { get; set; } = true;
    public AudioChannels Channels { get; }
    public int SampleRate { get; }
    public int BufferSize { get; }

    private int _currentBufferIndex = 0;
    private readonly List<float[]> _buffers = new(2);
    
    private AudioStream _stream;
    
    private readonly object _lockObj = new();
    private Task _updateTask;
    public RaylibAudioBuffers(AudioSettings settings)
    {
        Channels = settings.Channels;
        SampleRate = settings.SampleRate;
        BufferSize = settings.BufferSize;
        
        // Initialize the 2 buffers
        if (settings.Channels == AudioChannels.Mono)
        {
            _buffers.Add(new float[BufferSize]);
            _buffers.Add(new float[BufferSize]);
        }
        else if (settings.Channels == AudioChannels.Stereo)
        {
            _buffers.Add(new float[BufferSize * 2]); // Stereo has 2 channels
            _buffers.Add(new float[BufferSize * 2]);
        }
        else
        {
            throw new NotSupportedException($"Audio channel type '{settings.Channels}' is not supported.");
        }

        // Create the raylib stream
        rl.SetAudioStreamBufferSizeDefault(settings.BufferSize);
        
        _stream = rl.LoadAudioStream((uint)settings.SampleRate, (uint)32,
            (uint)((settings.Channels == AudioChannels.Stereo) ? 2 : 1));

        rl.PlayAudioStream(_stream);
        
        _updateTask = Task.Run(UpdateStream);
    }
    
    private async Task UpdateStream()
    {
        float phase = 0.0f;
        
        while (Enabled)
        {
            // Fill the buffer with a sine wave for testing purposes
            if (_buffers.Count == 0)
            {
                await Task.Delay(10);
                continue;
            }
            
            lock (_lockObj)
            {
                var bufferSpan = _buffers[_currentBufferIndex].AsSpan();
                int bufferSize = (Channels == AudioChannels.Mono) ? BufferSize : BufferSize * 2;
                for (int i = 0; i < bufferSize; i++)
                {
                    // Generate a simple sine wave for testing
                    bufferSpan[i] = (float)Math.Sin(phase);
                    phase += (MathF.PI * 440.0f) / SampleRate; // 440 Hz sine wave
                    if (phase >= MathF.PI * 2) phase -= MathF.PI * 2; // Wrap around!!!!
                }
            }
            
            
            // Wait for the audio stream to be ready
            while (!rl.IsAudioStreamProcessed(_stream))
            {
                await Task.Delay(10);
            }

            lock (_lockObj)
            {
                // Update the audio stream with the current buffer
                var bufferSpan = _buffers[_currentBufferIndex].AsSpan();
                unsafe
                {
                    fixed (float* ptr = &bufferSpan[0])
                    {
                        rl.UpdateAudioStream(_stream, ptr, BufferSize);
                    }
                }
            }
        }
    }

    public void SetBuffer(Span<float> buffer)
    {
        if (buffer.Length != BufferSize)
        {
            throw new ArgumentException($"Buffer size must be {BufferSize} samples, but received {buffer.Length} samples.");
        }

        // Copy the buffer into _buffers[_currentBufferIndex]
        buffer.CopyTo(_buffers[_currentBufferIndex]);
        
        OnBuffersUpdated?.Invoke(this);

        _currentBufferIndex = (_currentBufferIndex + 1) % _buffers.Count;
    }

    public Action<IAudioBuffers>? OnBuffersUpdated { get; set; }
    public void Close()
    {
        Enabled = false;
        _updateTask.Wait(); // Wait for the update task to finish
        
        // Stop the audio stream
        rl.StopAudioStream(_stream);
        
        // Unload the audio stream
        rl.UnloadAudioStream(_stream);
        
        // Clear the buffers
        _buffers.Clear();
        
        // Close the audio device
        rl.CloseAudioDevice();
    }
}