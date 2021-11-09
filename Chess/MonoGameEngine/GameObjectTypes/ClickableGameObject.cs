using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine
{
    class ClickableGameObject : TexturedGameObject, IClickable
    {
        private bool _isHovered;
        private bool _previousIsHovered;

        public event Action OnHoverEnter;
        public event Action OnHover;
        public event Action OnHoverExit;
        public event Action OnLeftMouseDown;
        public event Action OnLeftMouse;
        public event Action OnLeftMouseUp;
        public event Action OnMiddleMouseDown;
        public event Action OnMiddleMouse;
        public event Action OnMiddleMouseUp;
        public event Action OnRightMouseDown;
        public event Action OnRightMouse;
        public event Action OnRightMouseUp;

        public Color DisabledTint { get; set; } = Color.Gray;

        /// <summary>
        /// Rectangle where the GameObject can be clicked.
        /// Set to null to make it the size of the texture.
        /// </summary>
        public Rectangle? ClickableArea { get; set; } = null;

        public bool TintOnHover { get; set; } = false;

        public Color HoverTint { get; set; } = Color.Yellow;


        public ClickableGameObject() : base()
        {

        }


        protected override void OnUpdate(GameTime gameTime)
        {
            // Update hover states
            Rectangle hoverRectangle = GetHoverRectangle();
            _previousIsHovered = _isHovered;
            _isHovered = hoverRectangle.Contains(Mouse.Position);

            // Check and invoke mouse events
            if (_isHovered && !_previousIsHovered)
            {
                OnHoverEnter?.Invoke();
            }
            if (_isHovered)
            {
                OnHover?.Invoke();

                if (Mouse.IsLeftMouseDown)
                {
                    OnLeftMouseDown?.Invoke();
                }
                if (Mouse.IsLeftMouse)
                {
                    OnLeftMouse?.Invoke();
                }
                if (Mouse.IsLeftMouseUp)
                {
                    OnLeftMouseUp?.Invoke();
                }
                if (Mouse.IsMiddleMouseDown)
                {
                    OnMiddleMouseDown?.Invoke();
                }
                if (Mouse.IsMiddleMouse)
                {
                    OnMiddleMouse?.Invoke();
                }
                if (Mouse.IsMiddleMouseUp)
                {
                    OnMiddleMouseUp?.Invoke();
                }
                if (Mouse.IsRightMouseDown)
                {
                    OnRightMouseDown?.Invoke();
                }
                if (Mouse.IsRightMouse)
                {
                    OnRightMouse?.Invoke();
                }
                if (Mouse.IsRightMouseUp)
                {
                    OnRightMouseUp?.Invoke();
                }
            }
            if (!_isHovered && _previousIsHovered)
            {
                OnHoverExit?.Invoke();
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            // Determine draw tint
            Color drawTint = Tint;

            if (Enabled)
            {
                if (TintOnHover && _isHovered)
                {
                    drawTint = HoverTint;
                }
            }
            else
            {
                drawTint = DisabledTint;
            }

            // Draw with this determined tint
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, new Vector2(Transform.GlobalPosition.X, Transform.GlobalPosition.Y), SourceRectangle, drawTint, Transform.GlobalRotation, Transform.Origin, Transform.GlobalScale, SpriteEffects, Transform.GlobalPosition.Z);
            }
        }


        // Helper function that calculates the area where the object is being hovered over
        private Rectangle GetHoverRectangle()
        {
            // If a clickable area is defined, use that as the hover rectangle
            if (ClickableArea.HasValue)
            {
                int x = (int)(Transform.GlobalPosition.X - Transform.Origin.X + ClickableArea.Value.X);
                int y = (int)(Transform.GlobalPosition.Y - Transform.Origin.Y + ClickableArea.Value.Y);
                int width = (int)(ClickableArea.Value.Width * Transform.Scale.X);
                int height = (int)(ClickableArea.Value.Height * Transform.Scale.Y);

                return new Rectangle(x, y, width, height);
            }
            // If no clickable area defined, make the hover rectangle the dimensions of the texture
            else if (!ClickableArea.HasValue && Texture != null)
            {
                int x = (int)(Transform.GlobalPosition.X - Transform.Origin.X);
                int y = (int)(Transform.GlobalPosition.Y - Transform.Origin.Y);
                int width = (int)(Texture.Bounds.Width * Transform.Scale.X);
                int height = (int)(Texture.Bounds.Height * Transform.Scale.Y);

                return new Rectangle(x, y, width, height);
            }
            // If no clickable area defined and no texture, object cannot be clicked
            else
            {
                return new Rectangle();
            }
        }
    }
}
