using Solstice.Audio.Interfaces;
using Solstice.Engine.Classes;
using Solstice.Engine.Interfaces;
using Solstice.Graphics.Interfaces;

namespace Solstice.Engine.Components;

public class AudioListenerComponent : Component, IInitAudio
{
    /// <summary>
    /// If true, this component will take the parent's transform into account when calculating the audio.
    /// </summary>
    public bool IsPositional { get; set; } = true;
    private IAudio _audio;
    
    public override void Setup()
    {
        
    }

    public override void Update(IWindow window)
    {
        if (IsPositional)
        {
            _audio.AudioContext.IsPositional = true;
            _audio.AudioContext.SetListenerPosition(Owner.Transform.Position);
            _audio.AudioContext.SetListenerRotation(Owner.Transform.Rotation);
        }
        else
        {
            _audio.AudioContext.IsPositional = false;
        }
    }

    public override void Start()
    {
        
    }

    public void InitAudio(IAudio audio)
    {
        _audio = audio;
    }
}