using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess
{
    class GameObject : AppObject, Interfaces.IDrawable
    {
        private Texture2D sprite;
        private Rectangle rectangle;

        public Action OnMove;
        public Action OnResize;

        public Texture2D Sprite => sprite;

        public Rectangle Rectangle
        {
            get => rectangle;
            set
            {
                if (rectangle.X != value.X || rectangle.Y != value.Y)
                    OnMove?.Invoke();
                if (rectangle.Width != value.Width || rectangle.Height != value.Height)
                    OnResize?.Invoke();

                rectangle = value;
            }
        }

        public bool Visible { get; set; }

        public Color DrawColor { get; set; }

        public GameObject() : base(Vector2.Zero)
        {
            this.sprite = null;
            DrawColor = Color.White;
            Visible = false;

            rectangle = Rectangle.Empty;
        }

        public GameObject(Texture2D sprite) : base(Vector2.Zero)
        {
            this.sprite = sprite;
            DrawColor = Color.White;
            Visible = true;

            rectangle = Rectangle.Empty;
        }

        public GameObject(Texture2D sprite, Rectangle rectangle) : base(new Vector2(rectangle.X, rectangle.Y))
        {
            this.sprite = sprite;
            DrawColor = Color.White;
            Visible = true;

            this.rectangle = rectangle;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(this.Sprite, this.Rectangle, DrawColor);
        }
    }
}
