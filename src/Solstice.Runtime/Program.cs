using Solstice.Audio;
using Solstice.Audio.Enums;
using Solstice.Audio.Implementations;
using Solstice.Audio.Implementations.Generators;
using Solstice.Audio.Utilities;
using Solstice.Common;
using Solstice.Engine.Classes;
using Solstice.Graphics;
using Solstice.Graphics.Enums;

var scene = new Scene();

var window = WindowFactory.CreateWindow(WindowSettings.Default with
{
    Title = "Runtime",
    VSyncEnabled = true,
});

var graphics = window.Graphics;

var audio = AudioFactory.CreateAudio(AudioSettings.Default); // Creates a thread.

// audio.MasterChannel.Sources.Add(new AudioSamplerGenerator(AudioLoaders.LoadFile("./song.mp3", true), channels: 2,
//     looping: true, amplitude: 0.5f));

window.OnUpdate += (w) =>
{
    
};

window.Run();
audio.Close();