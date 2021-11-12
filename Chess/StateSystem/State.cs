using Microsoft.Xna.Framework;

namespace Chess
{
    abstract class State
    {
        public abstract void Enter();
        public abstract void Update(GameTime gameTime);
        public abstract void Exit();
    }
}
