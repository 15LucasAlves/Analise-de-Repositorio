using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine
{
    class DrawableGameObject : LoadableGameObject, IDrawable
    {
        public bool Visible { get; set; } = true;

        public Color Tint { get; set; } = Color.White;

        public SpriteEffects SpriteEffects { get; set; }


        public DrawableGameObject() : base()
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

            foreach (var child in Transform.Children)
            {
                if (child.GameObject is DrawableGameObject)
                {
                    (child.GameObject as DrawableGameObject).Draw(spriteBatch);
                }
            }
        }

        // Sets both Enabled and Visible
        public void SetActive(bool active)
        {
            Enabled = active;
            Visible = active;
        }
    }
}
