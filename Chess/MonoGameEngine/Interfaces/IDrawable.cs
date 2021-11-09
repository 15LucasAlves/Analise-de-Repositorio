using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine
{
    interface IDrawable
    {
        bool Visible { get; set; }
        void Draw(SpriteBatch spriteBatch);
    }
}