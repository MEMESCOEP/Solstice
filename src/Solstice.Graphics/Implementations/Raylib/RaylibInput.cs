using System.Numerics;
using Hexa.NET.Raylib;
using Solstice.Graphics.Enums;
using Solstice.Graphics.Interfaces;
using MouseButton = Solstice.Graphics.Enums.MouseButton;

namespace Solstice.Graphics.Implementations;

public class RaylibInput : IInput
{
    public bool IsKeyPressed(KeyCode key)
    {
        return Raylib.IsKeyPressed((int)key.ToRaylib());
    }

    public bool IsKeyReleased(KeyCode key)
    {
        return Raylib.IsKeyReleased((int)key.ToRaylib());
    }

    public bool IsKeyDown(KeyCode key)
    {
        return Raylib.IsKeyDown((int)key.ToRaylib());
    }

    public bool IsMouseButtonPressed(MouseButton button)
    {
        return Raylib.IsMouseButtonPressed((int)button.ToRaylib());
    }

    public bool IsMouseButtonReleased(MouseButton button)
    {
        return Raylib.IsMouseButtonReleased((int)button.ToRaylib());
    }

    public bool IsMouseButtonDown(MouseButton button)
    {
        return Raylib.IsMouseButtonDown((int)button.ToRaylib());
    }

    public Vector2 GetMousePosition()
    {
        return Raylib.GetMousePosition();
    }

    public Vector2 GetMouseDelta()
    {
        return Raylib.GetMouseDelta();
    }

    public Vector2 GetMouseScrollDelta()
    {
        return Raylib.GetMouseWheelMoveV();
    }

    private MouseState _currentMouseState;
    public void SetMouseState(MouseState state)
    {
        if (_currentMouseState == state)
            return;
        
        _currentMouseState = state;
        
        switch (state)
        {
            case MouseState.Normal:
                Raylib.EnableCursor();
                Raylib.ShowCursor();
                break;
            case MouseState.Locked:
                Raylib.DisableCursor();
                Raylib.HideCursor();
                break;
            case MouseState.Hidden:
                Raylib.HideCursor();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public MouseState GetMouseState()
    {
        return _currentMouseState;
    }
}