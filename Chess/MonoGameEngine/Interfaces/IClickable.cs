using System;

namespace MonoGameEngine
{
    interface IClickable
    {
        event Action OnHoverEnter;
        event Action OnHover;
        event Action OnHoverExit;
        event Action OnLeftMouseDown;
        event Action OnLeftMouse;
        event Action OnLeftMouseUp;
        event Action OnMiddleMouseDown;
        event Action OnMiddleMouse;
        event Action OnMiddleMouseUp;
        event Action OnRightMouseDown;
        event Action OnRightMouse;
        event Action OnRightMouseUp;
    }
}
