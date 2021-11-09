namespace MonoGameEngine
{
    static class AppManager
    {
        public static MonoGameApp AppRunning { get; private set; }


        /// <summary>
        /// To use this method, call it with T as a class that inherits from MonoGameApp.
        /// It will set it as the current AppRunning, close any previous open apps (if any), then call its Run method.
        /// </summary>
        public static void StartApp<T>() where T : MonoGameApp, new()
        {
            if (AppRunning != null)
            {
                AppRunning.Exit();
            }

            AppRunning = new T();
            AppRunning.Run();
        }

        public static void StopApp()
        {
            if (AppRunning != null)
            {
                AppRunning.Exit();
                AppRunning = null;
            }
        }
    }
}