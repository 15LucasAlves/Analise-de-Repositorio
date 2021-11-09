using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameEngine
{
    static class Input
    {
        // Mouse
        public static MouseState MouseState { get; private set; }
        public static MouseState PreviousMouseState { get; private set; }
        public static Point MousePosition => MouseState.Position;
        public static int MouseX => MouseState.X;
        public static int MouseY => MouseState.Y;
        public static int MouseScrollWheelValue => MouseState.ScrollWheelValue;
        public static bool IsLeftMouseDown => MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
        public static bool IsLeftMouse => MouseState.LeftButton == ButtonState.Pressed;
        public static bool IsLeftMouseUp => MouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed;
        public static bool IsMiddleMouseDown => MouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released;
        public static bool IsMiddleMouse => MouseState.MiddleButton == ButtonState.Pressed;
        public static bool IsMiddleMouseUp => MouseState.MiddleButton == ButtonState.Released && PreviousMouseState.MiddleButton == ButtonState.Pressed;
        public static bool IsRightMouseDown => MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
        public static bool IsRightMouse => MouseState.RightButton == ButtonState.Pressed;
        public static bool IsRightMouseUp => MouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed;


        // Keyboard
        public static KeyboardState KeyboardState { get; private set; }
        public static KeyboardState PreviousKeyboardState { get; private set; }
        public static bool IstKeyDown(Keys key) => KeyboardState.IsKeyDown(key) && !PreviousKeyboardState.IsKeyDown(key);
        public static bool IsKey(Keys key) => KeyboardState.IsKeyDown(key);
        public static bool IsKeyUp(Keys key) => !KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyDown(key);
        public static bool IsKeyCombinationDown(params Keys[] keys)
        {
            // All keys except the last one must be down
            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (!KeyboardState.IsKeyDown(keys[i]))
                {
                    return false;
                }
            }

            // True only if last key was pressed down this frame
            if (IstKeyDown(keys[keys.Length - 1]))
            {
                return true;
            }

            return false;
        }


        public static void Update(GameTime gameTime)
        {
            PreviousMouseState = MouseState;
            MouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            PreviousKeyboardState = KeyboardState;
            KeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }
    }
}
