using Microsoft.Xna.Framework;

namespace MonoGameEngine
{
    class AppObject : IUpdatable
    {
        public bool Enabled { get; set; } = true;


        public AppObject()
        {

        }

        /// <summary>
        /// Does nothing by default. Override for functionality.
        /// Called regularly. Implement game logic here.
        /// </summary>
        protected virtual void OnUpdate(GameTime gameTime)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (!Enabled || gameTime == null)
            {
                return;
            }

            OnUpdate(gameTime);
        }
    }
}