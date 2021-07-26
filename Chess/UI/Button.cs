using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.UI
{
    // Buttons to interact with menus.
    class Button : ClickableSprite
    {
        private StringObject text;

        public string Text
        {
            get => text.Text;
            set => text.Text = value;
        }

        public Button(Texture2D sprite, Rectangle rectangle, Color hoverTint, string text, SpriteFont font) : base(sprite, rectangle, hoverTint)
        {
            TintOnHover = true;
            this.text = new StringObject(text, font, new Vector2(this.Rectangle.X + 10, this.Rectangle.Y + 10));
        }

        public Button(Texture2D sprite, Rectangle rectangle, Color hoverTint, string text, SpriteFont font, Action clickFunction) : base(sprite, rectangle, hoverTint, clickFunction)
        {
            TintOnHover = true;
            this.text = new StringObject(text, font, new Vector2(this.Rectangle.X + 10, this.Rectangle.Y + 10));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Visible)
                text.Draw(spriteBatch);
        }
    }
}
