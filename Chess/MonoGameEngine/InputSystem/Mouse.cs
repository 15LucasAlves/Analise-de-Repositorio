using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameEngine
{
    // Shortcut/Alias for Mouse functions in Input class
    static class Mouse
    {
        public static MouseState MouseState => Input.MouseState;
        public static MouseState PreviousMouseState => Input.PreviousMouseState;
        public static Point Position => Input.MousePosition;
        public static int X => Input.MouseX;
        public static int Y => Input.MouseY;
        public static int ScrollWheelValue => Input.MouseScrollWheelValue;
        public static bool IsLeftMouseDown => Input.IsLeftMouseDown;
        public static bool IsLeftMouse => Input.IsLeftMouse;
        public static bool IsLeftMouseUp => Input.IsLeftMouseUp;
        public static bool IsMiddleMouseDown => Input.IsMiddleMouseDown;
        public static bool IsMiddleMouse => Input.IsMiddleMouse;
        public static bool IsMiddleMouseUp => Input.IsMiddleMouseUp;
        public static bool IsRightMouseDown => Input.IsRightMouseDown;
        public static bool IsRightMouse => Input.IsRightMouse;
        public static bool IsRightMouseUp => Input.IsRightMouseUp;
    }
}
