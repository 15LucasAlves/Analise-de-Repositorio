namespace MonoGameEngine
{
    class SceneManager
    {
        private MonoGameApp _app;
        

        public Scene CurrentScene { get; private set; }


        public SceneManager(MonoGameApp app)
        {
            _app = app;
        }


        /// <summary>
        /// To use this method, call it with T as a class that inherits from Scene.
        /// It will set it as the Current Scene, then it will call its Load & Initialize methods.
        /// </summary>
        public void LoadScene<T>() where T : Scene, new()
        {
            CurrentScene = new T();
            CurrentScene.Load(_app);
        }
    }
}