using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Chess.Managers;

namespace Chess
{
    class ClickableObject : GameObject, Interfaces.IUpdatable
    {
        public Action OnClick;

        public Action OnHover;
        public Action OnHoverEnter;
        public Action OnHoverExit;

        private bool hoverState;
        private bool previousHoverState;

        private readonly Color DisabledColor = Color.Gray;

        // Whether object can be clicked.
        public bool Enabled { get; set; }

        private bool tintOnHover;
        // Whether object will receive a tint when hovered over
        public bool TintOnHover
        {
            get => tintOnHover;
            set
            {
                if (value)
                {
                    OnHoverEnter += ChangeDrawColorToHover;
                    OnHoverExit += ResetDrawColor;
                }
                else
                {
                    OnHoverEnter -= ChangeDrawColorToHover;
                    OnHoverExit -= ResetDrawColor;
                }

                tintOnHover = value;
            }
        }

        // Tint object will be receive when hovered over.
        public Color HoverColor { get; set; }

        public ClickableObject(Rectangle rectangle) : base()
        {
            hoverState = false;
            previousHoverState = false;
            Enabled = true;
            TintOnHover = false;
        }

        public ClickableObject(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            hoverState = false;
            previousHoverState = false;
            Enabled = true;
            TintOnHover = false;
        }

        public ClickableObject(Texture2D sprite, Rectangle rectangle, Action clickFunction) : base(sprite, rectangle)
        {
            hoverState = false;
            previousHoverState = false;
            Enabled = true;
            TintOnHover = false;
            OnClick += clickFunction;
        }

        public ClickableObject(Texture2D sprite, Rectangle rectangle, Color hoverColor) : base(sprite, rectangle)
        {
            hoverState = false;
            previousHoverState = false;
            Enabled = true;
            TintOnHover = true;
            HoverColor = hoverColor;
        }

        public ClickableObject(Texture2D sprite, Rectangle rectangle, Color hoverColor, Action clickFunction) : base(sprite, rectangle)
        {
            hoverState = false;
            previousHoverState = false;
            Enabled = true;
            TintOnHover = true;
            HoverColor = hoverColor;
            OnClick += clickFunction;
        }

        public virtual void Update(GameTime gameTime)
        {
            previousHoverState = hoverState;
            hoverState = CheckHover(AppManager.MouseState.X, AppManager.MouseState.Y);

            if (previousHoverState && hoverState)
                OnHover?.Invoke();

            if (!previousHoverState && hoverState)
                OnHoverEnter?.Invoke();

            if (previousHoverState && !hoverState)
                OnHoverExit?.Invoke();

            if (Enabled)
                CheckClick();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                if (Enabled)
                    spriteBatch.Draw(this.Sprite, this.Rectangle, DrawColor);
                else
                    spriteBatch.Draw(this.Sprite, this.Rectangle, DisabledColor);
            }
        }

        #region Helper Methods
        public bool CheckHover(int x, int y)
        {
            return Rectangle.Contains(x, y);
        }

        private void CheckClick()
        {
            if (hoverState && AppManager.LeftMouseUp)
                OnClick?.Invoke();
        }

        private void ChangeDrawColorToHover()
        {
            DrawColor = HoverColor;
        }

        private void ResetDrawColor()
        {
            DrawColor = Color.White;
        }
        #endregion
    }
}
