namespace MonoGameEngine
{
    class LoadableAppObject : AppObject, ILoadable
    {
        public LoadableAppObject() : base()
        {

        }


        /// <summary>
        /// Does nothing by default. Override for functionality.
        /// Called right after Scene is Loaded. Set Content here (like Texture2D's & SpriteFont's).
        /// </summary>
        protected virtual void OnLoad(MonoGameApp app)
        {
        }

        public virtual void Load(MonoGameApp app)
        {
            if (app == null)
            {
                return;
            }

            OnLoad(app);
        }
    }
}
