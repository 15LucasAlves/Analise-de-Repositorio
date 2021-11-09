namespace MonoGameEngine
{
    class LoadableGameObject : GameObject, ILoadable
    {
        public LoadableGameObject() : base()
        {

        }


        /// <summary>
        /// Does nothing by default. Override for functionality.
        /// Called right after Scene is Loaded. Set Content here (like Texture2D's & SpriteFont's).
        /// </summary>
        protected virtual void OnLoad(MonoGameApp app)
        {

        }

        public void Load(MonoGameApp app)
        {
            if (app == null)
            {
                return;
            }

            OnLoad(app);

            foreach (var child in Transform.Children)
            {
                if (child.GameObject is LoadableGameObject)
                {
                    (child.GameObject as LoadableGameObject).Load(app);
                }
            }
        }
    }
}
