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
        _buffers.Add(new float[BufferSize]);
        _buffers.Add(new float[BufferSize]);
        
        // Create the raylib stream
        rl.SetAudioStreamBufferSizeDefault(settings.BufferSize);
        
        _stream = rl.LoadAudioStream((uint)settings.SampleRate, (uint)settings.BufferSize,
            (uint)((settings.Channels == AudioChannels.Stereo) ? 2 : 1));

        rl.PlayAudioStream(_stream);
        
        _updateTask = Task.Run(UpdateStream);
    }
    
    private async Task UpdateStream()
    {
        while (Enabled)
        {
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
                    fixed (float* ptr = bufferSpan)
                    {
                        rl.UpdateAudioStream(_stream, ptr,
                            (Channels == AudioChannels.Stereo ? BufferSize * 2 : BufferSize));
                    }
                }

                _buffers[_currentBufferIndex] = new float[BufferSize]; // Reset the buffer for the next use
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