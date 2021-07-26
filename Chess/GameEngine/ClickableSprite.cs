using System;
using Microsoft.Xna.Framework;

namespace Chess
{
    class ClickableSprite : Sprite
    {
        private bool previousHoverState;
        public bool IsHovered { get; private set; }

        public event Action OnMouse;
        public event Action OnMouseDown;
        public event Action OnMouseUp;

        public event Action OnHover;
        public event Action OnHoverEnter;
        public event Action OnHoverExit;

        public override void Update(GameTime gameTime)
        {
            previousHoverState = IsHovered;

            Rectangle boundingRect = new Rectangle(Position, Scale);
            IsHovered = Collision.AreColliding(boundingRect, SmartMouse.Position);

            // Hover events
            if (IsHovered)
                OnHover?.Invoke();

            if (!previousHoverState && IsHovered)
                OnHoverEnter?.Invoke();

            if (previousHoverState && !IsHovered)
                OnHoverExit?.Invoke();

            // Mouse events
            if (IsHovered && SmartMouse.LeftMouse)
            {
                OnMouse?.Invoke();
            }

            if (IsHovered && SmartMouse.LeftMouseDown)
            {
                OnMouseDown?.Invoke();
            }

            if (IsHovered && SmartMouse.LeftMouseUp)
            {
                OnMouseUp?.Invoke();
            }
        }
    }
}
