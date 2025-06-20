using System.Numerics;
using Solstice.Graphics.Enums;

namespace Solstice.Graphics.Interfaces;

public interface IInput
{
    /// <summary>
    /// If the key was pressed this frame, but not the previous frame, this will return true.
    /// </summary>
    public bool IsKeyPressed(KeyCode key);
    
    /// <summary>
    /// If the key was up this frame, but down the previous frame, this will return true.
    /// </summary>
    public bool IsKeyReleased(KeyCode key);
    
    /// <summary>
    /// If the key is currently down, this will return true.
    /// </summary>
    public bool IsKeyDown(KeyCode key);
    
    /// <summary>
    /// If the mouse button was pressed this frame, but not the previous frame, this will return true.
    /// </summary>
    public bool IsMouseButtonPressed(MouseButton button);
    
    /// <summary>
    /// If the mouse button was up this frame, but down the previous frame, this will return true.
    /// </summary>
    public bool IsMouseButtonReleased(MouseButton button);
    
    /// <summary>
    /// If the mouse button is currently down, this will return true.
    /// </summary>
    public bool IsMouseButtonDown(MouseButton button);
    
    /// <summary>
    /// The current position of the mouse in screen coordinates.
    /// </summary>
    public Vector2 GetMousePosition();
    
    /// <summary>
    /// The difference in mouse position since the last frame.
    /// </summary>
    public Vector2 GetMouseDelta();
    
    /// <summary>
    /// The current scroll delta of the mouse wheel. (X is horizontal, Y is vertical)
    /// </summary>
    public Vector2 GetMouseScrollDelta();
    
    public void SetMouseState(MouseState state);
    public MouseState GetMouseState();
}