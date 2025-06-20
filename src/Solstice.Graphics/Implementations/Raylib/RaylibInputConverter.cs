using Hexa.NET.Raylib;
using Solstice.Graphics.Enums;
using MouseButton = Solstice.Graphics.Enums.MouseButton;

namespace Solstice.Graphics.Implementations;

public static class RaylibInputConverter
{
    private static readonly Dictionary<KeyCode, KeyboardKey> _keyboardMap = new()
    {
        // Alphabet
        { KeyCode.A, KeyboardKey.A },
        { KeyCode.B, KeyboardKey.B },
        { KeyCode.C, KeyboardKey.C },
        { KeyCode.D, KeyboardKey.D },
        { KeyCode.E, KeyboardKey.E },
        { KeyCode.F, KeyboardKey.F },
        { KeyCode.G, KeyboardKey.G },
        { KeyCode.H, KeyboardKey.H },
        { KeyCode.I, KeyboardKey.I },
        { KeyCode.J, KeyboardKey.J },
        { KeyCode.K, KeyboardKey.K },
        { KeyCode.L, KeyboardKey.L },
        { KeyCode.M, KeyboardKey.M },
        { KeyCode.N, KeyboardKey.N },
        { KeyCode.O, KeyboardKey.O },
        { KeyCode.P, KeyboardKey.P },
        { KeyCode.Q, KeyboardKey.Q },
        { KeyCode.R, KeyboardKey.R },
        { KeyCode.S, KeyboardKey.S },
        { KeyCode.T, KeyboardKey.T },
        { KeyCode.U, KeyboardKey.U },
        { KeyCode.V, KeyboardKey.V },
        { KeyCode.W, KeyboardKey.W },
        { KeyCode.X, KeyboardKey.X },
        { KeyCode.Y, KeyboardKey.Y },
        { KeyCode.Z, KeyboardKey.Z },

        // Numbers
        { KeyCode.Num0, KeyboardKey.Zero },
        { KeyCode.Num1, KeyboardKey.One },
        { KeyCode.Num2, KeyboardKey.Two },
        { KeyCode.Num3, KeyboardKey.Three },
        { KeyCode.Num4, KeyboardKey.Four },
        { KeyCode.Num5, KeyboardKey.Five },
        { KeyCode.Num6, KeyboardKey.Six },
        { KeyCode.Num7, KeyboardKey.Seven },
        { KeyCode.Num8, KeyboardKey.Eight },
        { KeyCode.Num9, KeyboardKey.Nine },

        // Numpad
        { KeyCode.Numpad0, KeyboardKey.Kp0 },
        { KeyCode.Numpad1, KeyboardKey.Kp1 },
        { KeyCode.Numpad2, KeyboardKey.Kp2 },
        { KeyCode.Numpad3, KeyboardKey.Kp3 },
        { KeyCode.Numpad4, KeyboardKey.Kp4 },
        { KeyCode.Numpad5, KeyboardKey.Kp5 },
        { KeyCode.Numpad6, KeyboardKey.Kp6 },
        { KeyCode.Numpad7, KeyboardKey.Kp7 },
        { KeyCode.Numpad8, KeyboardKey.Kp8 },
        { KeyCode.Numpad9, KeyboardKey.Kp9 },
        { KeyCode.NumpadDecimal, KeyboardKey.KpDecimal },
        { KeyCode.NumpadDivide, KeyboardKey.KpDivide },
        { KeyCode.NumpadMultiply, KeyboardKey.KpMultiply },
        { KeyCode.NumpadSubtract, KeyboardKey.KpSubtract },
        { KeyCode.NumpadAdd, KeyboardKey.KpAdd },
        { KeyCode.NumpadEnter, KeyboardKey.KpEnter },

        // F1 - F12
        { KeyCode.F1, KeyboardKey.F1 },
        { KeyCode.F2, KeyboardKey.F2 },
        { KeyCode.F3, KeyboardKey.F3 },
        { KeyCode.F4, KeyboardKey.F4 },
        { KeyCode.F5, KeyboardKey.F5 },
        { KeyCode.F6, KeyboardKey.F6 },
        { KeyCode.F7, KeyboardKey.F7 },
        { KeyCode.F8, KeyboardKey.F8 },
        { KeyCode.F9, KeyboardKey.F9 },
        { KeyCode.F10, KeyboardKey.F10 },
        { KeyCode.F11, KeyboardKey.F11 },
        { KeyCode.F12, KeyboardKey.F12 },

        // Special keys
        { KeyCode.Escape, KeyboardKey.Escape },
        { KeyCode.Tab, KeyboardKey.Tab },
        { KeyCode.CapsLock, KeyboardKey.CapsLock },
        { KeyCode.LeftShift, KeyboardKey.LeftShift },
        { KeyCode.LeftControl, KeyboardKey.LeftControl },
        { KeyCode.LeftAlt, KeyboardKey.LeftAlt },
        { KeyCode.LeftSuper, KeyboardKey.LeftSuper },
        { KeyCode.RightShift, KeyboardKey.RightShift },
        { KeyCode.RightControl, KeyboardKey.RightControl },
        { KeyCode.RightAlt, KeyboardKey.RightAlt },
        { KeyCode.RightSuper, KeyboardKey.RightSuper },
        { KeyCode.PrintScreen, KeyboardKey.PrintScreen },
        { KeyCode.ScrollLock, KeyboardKey.ScrollLock },
        { KeyCode.Pause, KeyboardKey.Pause },
        { KeyCode.Insert, KeyboardKey.Insert },
        { KeyCode.Home, KeyboardKey.Home },
        { KeyCode.PageUp, KeyboardKey.PageUp },
        { KeyCode.Delete, KeyboardKey.Delete },
        { KeyCode.End, KeyboardKey.End },
        { KeyCode.PageDown, KeyboardKey.PageDown },
        { KeyCode.RightArrow, KeyboardKey.Right },
        { KeyCode.LeftArrow, KeyboardKey.Left },
        { KeyCode.DownArrow, KeyboardKey.Down },
        { KeyCode.UpArrow, KeyboardKey.Up },

        // Miscellaneous
        { KeyCode.Space, KeyboardKey.Space },
        { KeyCode.Enter, KeyboardKey.Enter },
        { KeyCode.Backspace, KeyboardKey.Backspace },
        { KeyCode.Grave, KeyboardKey.Grave },
        { KeyCode.Minus, KeyboardKey.Minus },
        { KeyCode.Equals, KeyboardKey.Equal },
        { KeyCode.NumLock, KeyboardKey.NumLock },
        { KeyCode.LeftBracket, KeyboardKey.LeftBracket },
        { KeyCode.RightBracket, KeyboardKey.RightBracket },
        { KeyCode.Backslash, KeyboardKey.Backslash },
        { KeyCode.Semicolon, KeyboardKey.Semicolon },
        { KeyCode.Apostrophe, KeyboardKey.Apostrophe },
        { KeyCode.Comma, KeyboardKey.Comma },
        { KeyCode.Period, KeyboardKey.Period },
        { KeyCode.Slash, KeyboardKey.Slash },
    };
    
    private static readonly Dictionary<MouseButton, Hexa.NET.Raylib.MouseButton> _mouseMap = new()
    {
        { MouseButton.Left, Hexa.NET.Raylib.MouseButton.Left },
        { MouseButton.Middle, Hexa.NET.Raylib.MouseButton.Middle },
        { MouseButton.Right, Hexa.NET.Raylib.MouseButton.Right },
        { MouseButton.Extra1, Hexa.NET.Raylib.MouseButton.Forward },
        { MouseButton.Extra2, Hexa.NET.Raylib.MouseButton.Back }
    };

    public static KeyboardKey ToRaylib(this KeyCode keyCode)
    {
        if (_keyboardMap.TryGetValue(keyCode, out var keyboardKey))
        {
            return keyboardKey;
        }
        
        throw new ArgumentException($"KeyCode '{keyCode}' is not mapped to a valid KeyboardKey.");
    }
    
    public static KeyCode ToKeyCode(this KeyboardKey keyboardKey)
    {
        var entry = _keyboardMap.FirstOrDefault(kvp => kvp.Value == keyboardKey);
        if (entry.Key != default)
        {
            return entry.Key;
        }
        
        throw new ArgumentException($"KeyboardKey '{keyboardKey}' is not mapped to a valid KeyCode.");
    }
    
    public static Hexa.NET.Raylib.MouseButton ToRaylib(this MouseButton mouseButton)
    {
        if (_mouseMap.TryGetValue(mouseButton, out var raylibMouseButton))
        {
            return raylibMouseButton;
        }
        
        throw new ArgumentException($"MouseButton '{mouseButton}' is not mapped to a valid Raylib MouseButton.");
    }
    
    public static MouseButton ToMouseButton(this Hexa.NET.Raylib.MouseButton mouseButton)
    {
        var entry = _mouseMap.FirstOrDefault(kvp => kvp.Value == mouseButton);
        if (entry.Key != default)
        {
            return entry.Key;
        }
        
        throw new ArgumentException($"Raylib MouseButton '{mouseButton}' is not mapped to a valid MouseButton.");
    }
}