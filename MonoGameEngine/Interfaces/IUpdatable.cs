using Microsoft.Xna.Framework;

namespace Chess.Interfaces
{
    interface IUpdatable
    {
        bool Enabled { get; set; }
        void OnEnable();
        void OnDisable();
        void Update(GameTime gameTime);
    }
}