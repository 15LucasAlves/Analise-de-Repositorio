using Microsoft.Xna.Framework;

namespace MonoGameEngine
{
    interface IUpdatable
    {
        bool Enabled { get; set; }
        void Update(GameTime gameTime);
    }
}