using Solstice.Audio.Interfaces;
using Solstice.Engine.Classes;
using Solstice.Engine.Interfaces;
using Solstice.Graphics.Interfaces;

namespace Solstice.Engine.Components;

public class AudioSourceComponent : Component, IInitAudio
{
    private IAudioSource _audioSource; 
    private IAudio _audio;
    public bool IsPositional { get; set; } = true;
    public AudioSourceComponent(IChannel targetChannel, IAudioSource audioSource)
    {
        _audioSource = audioSource ?? throw new ArgumentNullException(nameof(audioSource), "Audio source cannot be null");
        targetChannel.Sources.Add(_audioSource);
    }
    
    public override void Setup()
    {
        
    }

    public override void Update(IWindow window)
    {
        _audioSource.IsPositional = IsPositional;
        _audioSource.Position = Owner.Transform.Position;
    }

    public override void Start()
    {
        
    }

    public void InitAudio(IAudio audio)
    {
        _audio = audio;
    }
}