using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameEngine
{
    abstract class MonoGameApp : Game
    {
        public GraphicsDeviceManager Graphics { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                Graphics.PreferredBackBufferWidth = _width;
                Graphics.ApplyChanges();
            }
        }

        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                Graphics.PreferredBackBufferHeight = _height;
                Graphics.ApplyChanges();
            }
        }

        public SceneManager SceneManager { get; private set; }

        public Color ClearColor { get; set; } = Color.CornflowerBlue;

        public Keys DebugModeKey { get; set; } = Keys.OemTilde;

        public SpriteFont DebugFont { get; set; }

        public Color DebugFontColor { get; set; } = Color.Black;


        private string[] _debugLogs = new string[] { };


        public MonoGameApp(int width = 800, int height = 600, string contentRootDirectory = "Content") : base()
        {
            Graphics = new GraphicsDeviceManager(this);
            SceneManager = new SceneManager(this);

            Width = width;
            Height = height;

            Content.RootDirectory = contentRootDirectory;
        }


        protected override void Initialize()
        {
            base.Initialize();

            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update inputs first
            Input.Update(gameTime);

            // Handle keyboard inputs
            HandleKeyboardInputs();

            // Update current scene and all updatable objects inside of it
            SceneManager.CurrentScene?.Update(gameTime);

            // Collect Debug Logs
            _debugLogs = DebugManager.DumpLogs();
        }

        private void HandleKeyboardInputs()
        {
            // Check for Debug Mode Toggle
            if (Keyboard.IsKeyUp(DebugModeKey))
            {
                DebugManager.ToggleDebugMode();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Clear screen
            GraphicsDevice.Clear(ClearColor);

            
            SpriteBatch.Begin();

            // Draw current scene and all drawable objects inside of it
            SceneManager.CurrentScene?.Draw(SpriteBatch);

            // Draw Debug if Debug Mode is on and a Debug Font has been supplied
            if (DebugManager.DebugMode && DebugFont != null)
            {
                DrawDebug();
            }

            SpriteBatch.End();
        }

        private void DrawDebug()
        {
            for (int i = 0; i < _debugLogs.Length; i++)
            {
                string logText = _debugLogs[i];
                float logTextHeight = DebugFont.MeasureString(logText).Y;
                Vector2 logTextPosition = new Vector2(0, i * logTextHeight);
                SpriteBatch.DrawString(DebugFont, logText, logTextPosition, DebugFontColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
            }
        }
    }
}
