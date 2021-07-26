using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

static class SmartMouse
{
    public static bool Enabled { get; set; }

    private static MouseState MouseState { get; set; }
    private static MouseState PreviousMouseState { get; set; }

    public static Point Position => MouseState.Position;
    public static event Action<MouseState> OnMouseMove;

    public static bool LeftMouseDown => MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
    public static event Action<MouseState> OnLeftMouseDown;

    public static bool LeftMouseUp => MouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed;
    public static event Action<MouseState> OnLeftMouseUp;
    public static bool LeftMouse => MouseState.LeftButton == ButtonState.Pressed;
    public static event Action<MouseState> OnLeftMouse;

    public static bool RightMouseDown => MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
    public static event Action<MouseState> OnRightMouseDown;

    public static bool RightMouseUp => MouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed;
    public static event Action<MouseState> OnRightMouseUp;

    public static bool RightMouse => MouseState.RightButton == ButtonState.Pressed;
    public static event Action<MouseState> OnRightMouse;

    public static void Update(GameTime gameTime)
    {
        Enabled = true;

        if (!Enabled) return;

        PreviousMouseState = MouseState;
        MouseState = Mouse.GetState();

        if (PreviousMouseState.Position != Position)
        {
            OnMouseMove?.Invoke(MouseState);
        }

        if (LeftMouseDown)
        {
            OnLeftMouseDown?.Invoke(MouseState);
        }

        if (LeftMouseUp)
        {
            OnLeftMouseUp?.Invoke(MouseState);
        }

        if (LeftMouse)
        {
            OnLeftMouse?.Invoke(MouseState);
        }

        if (RightMouseDown)
        {
            OnRightMouseDown?.Invoke(MouseState);
        }

        if (RightMouseUp)
        {
            OnRightMouseUp?.Invoke(MouseState);
        }

        if (RightMouse)
        {
            OnRightMouse?.Invoke(MouseState);
        }
    }
}