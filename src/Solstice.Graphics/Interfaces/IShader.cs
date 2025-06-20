namespace Solstice.Graphics.Interfaces;


/// <summary>
/// A shader is a GPU program that calculates and manipulates data. Often used for graphics, though can also be used for compute tasks.
/// </summary>
public interface IShader
{
    internal void Use();
}