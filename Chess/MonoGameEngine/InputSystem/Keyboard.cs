using Microsoft.Xna.Framework.Input;

namespace MonoGameEngine
{
    // Shortcut/Alias for Keyboard functions in Input class
    static class Keyboard
    {
        public static KeyboardState KeyboardState => Input.KeyboardState;
        public static KeyboardState PreviousKeyboardState => Input.PreviousKeyboardState;
        public static bool IsKeyDown(Keys key) => Input.IstKeyDown(key);
        public static bool IsKey(Keys key) => Input.IsKey(key);
        public static bool IsKeyUp(Keys key) => Input.IsKeyUp(key);
        public static bool IsKeyCombinationDown(params Keys[] keys) => Input.IsKeyCombinationDown(keys);
    }
}
