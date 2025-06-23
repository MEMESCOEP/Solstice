using System.Numerics;

namespace Solstice.Audio.Classes;

public class AudioContext
{
    public bool IsPositional { get; set; }

    private Vector3 _rawListenerPosition;
    private Quaternion _rawListenerRotation;

    private Vector3 _startListenerPosition;
    private Vector3 _targetListenerPosition;

    private Quaternion _startListenerRotation;
    private Quaternion _targetListenerRotation;

    private int _bufferSize = 2048;

    public void SetListenerPosition(Vector3 position)
    {
        _rawListenerPosition = position;
        if (_targetListenerPosition == Vector3.Zero)
        {
            _startListenerPosition = _targetListenerPosition = position;
        }
    }

    public void SetListenerRotation(Quaternion rotation)
    {
        _rawListenerRotation = rotation;
        if (_targetListenerRotation == Quaternion.Identity)
        {
            _startListenerRotation = _targetListenerRotation = rotation;
        }
    }

    public void Update(float dt, int bufferSize)
    {
        _bufferSize = bufferSize;

        // Move previous target to start for interpolation
        _startListenerPosition = _targetListenerPosition;
        _startListenerRotation = _targetListenerRotation;

        // Smoothly update the target values toward raw inputs
        if (_rawListenerPosition != Vector3.Zero)
        {
            _targetListenerPosition = Vector3.Lerp(_targetListenerPosition, _rawListenerPosition, dt * 50);
        }

        if (_rawListenerRotation != Quaternion.Identity)
        {
            _targetListenerRotation = Quaternion.Slerp(_targetListenerRotation, _rawListenerRotation, dt * 50);
        }
    }

    public Vector3 GetListenerPosition(int sample)
    {
        float t = (float)sample / _bufferSize;
        return Vector3.Lerp(_startListenerPosition, _targetListenerPosition, t);
    }

    public Quaternion GetListenerRotation(int sample)
    {
        float t = (float)sample / _bufferSize;
        return Quaternion.Slerp(_startListenerRotation, _targetListenerRotation, t);
    }
}
