using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine
{
    class TexturedGameObject : DrawableGameObject
    {
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Rectangle from Texture that will be drawn.
        /// Set to null to draw the whole Texture.
        /// </summary>
        public Rectangle? SourceRectangle { get; set; }


        public TexturedGameObject() : base()
        {

        }


        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, new Vector2(Transform.GlobalPosition.X, Transform.GlobalPosition.Y), SourceRectangle, Tint, Transform.GlobalRotation, Transform.Origin, Transform.GlobalScale, SpriteEffects, Transform.GlobalPosition.Z);
            }
        }
    }
}
