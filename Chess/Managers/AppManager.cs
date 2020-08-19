using Microsoft.Xna.Framework.Input;
using System;

namespace Chess.Managers
{
    public enum AppState { MainMenu, Game, End }

    static class AppManager
    {
        public static Action<AppState> OnStateChange;

        private static AppState appState;
        public static AppState AppState
        {
            get => appState;
            set
            {
                OnStateChange?.Invoke(value);
                appState = value;
            }
        }
        public static MouseState MouseState { get; set; }
        public static MouseState PreviousMouseState { get; set; }

        public static KeyboardState KeyboardState { get; set; }
        public static KeyboardState PreviousKeyboardState { get; set; }

        public static bool DebugMode { get; private set; } = false;


        public static bool LeftMouseDown => MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
        public static bool LeftMouseUp => MouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed;
        public static bool LeftMouseHold => MouseState.LeftButton == ButtonState.Pressed;

        public static bool RightMouseDown => MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
        public static bool RightMouseUp => MouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed;
        public static bool RightMouseHold => MouseState.RightButton == ButtonState.Pressed;


        public static void UpdateInputStates()
        {
            PreviousMouseState = MouseState;
            MouseState = Mouse.GetState();

            PreviousKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
        }

        // Returns whether this is the first frame the key was pressed
        public static bool SingleKeyPress(Keys key)
        {
            if (KeyboardState.IsKeyDown(key) && !PreviousKeyboardState.IsKeyDown(key))
                return true;
            else
                return false;
        }

        // Returns whether this is the first frame the key combination was pressed
        // The last parameter is the trigger key
        public static bool KeyCombinationDown(params Keys[] keys)
        {
            bool allKeysDown = true;

            for (int i = 0; i < keys.Length; i++)
            {
                if (!KeyboardState.IsKeyDown(keys[i]))
                    allKeysDown = false;
            }

            if (!SingleKeyPress(keys[keys.Length - 1]))
                allKeysDown = false;

            return allKeysDown;
        }

        public static void ToggleDebug()
        {
                DebugMode = !DebugMode;
        }
    }
}
