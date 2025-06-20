using Hexa.NET.Raylib;
using Solstice.Graphics.Interfaces;

namespace Solstice.Graphics.Implementations;

public class RaylibShader : IShader
{
    private Shader _rlshader;
    
    public RaylibShader(Shader rlshader)
    {
        _rlshader = rlshader;
    }
    
    public void Use()
    {
        
    }
}