using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine
{
    class DrawableAppObject : LoadableAppObject, IDrawable
    {
        public bool Visible { get; set; } = true;


        public DrawableAppObject() : base()
        {

        }


        /// <summary>
        /// Does nothing by default. Override for functionality.
        /// Called on every Scene Draw call. Implement Drawing logic here.
        /// </summary>
        protected virtual void OnDraw(SpriteBatch spriteBatch)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible || spriteBatch == null)
            {
                return;
            }

            OnDraw(spriteBatch);
        }

        // Sets both Enabled and Visible
        public void SetActive(bool active)
        {
            Enabled = active;
            Visible = active;
        }
    }
}
