using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine
{
    class TextGameObject : DrawableGameObject
    {
        public SpriteFont Font { get; set; }

        public string Text { get; set; }


        public TextGameObject() : base()
        {

        }


        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (Font != null)
            {
                spriteBatch.DrawString(Font, Text, new Vector2(Transform.GlobalPosition.X, Transform.GlobalPosition.Y), Tint, Transform.GlobalRotation, Transform.Origin, Transform.GlobalScale, SpriteEffects, Transform.GlobalPosition.Z);
            }
        }
    }
}
