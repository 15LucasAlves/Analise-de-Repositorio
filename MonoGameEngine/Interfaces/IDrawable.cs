using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine.Interfaces
{
    interface IDrawable
    {
        bool Visible { get; set; }
        Rectangle Rectangle { get; set; }
        void Draw(SpriteBatch spriteBatch);
    }
}