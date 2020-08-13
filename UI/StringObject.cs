using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.UI
{
    class StringObject : AppObject, Interfaces.IDrawable
    {
        public string Text;
        public SpriteFont Font;

        public StringObject(string text, SpriteFont font, Vector2 position) : base(position)
        {
            Text = text;
            Font = font;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.DrawString(Font, Text, Position, Color.White);
        }
    }
}
