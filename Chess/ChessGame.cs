using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameEngine;

namespace Chess
{
    class ChessGame : MonoGameApp
    {
        private ChessButtonCreator _chessButtonCreator = new ChessButtonCreator();

        public Button CreateButton(string text = "", Vector3 position = default(Vector3)) => _chessButtonCreator.CreateButton(text, position);


        public ChessGame() : base(800, 600, "Content") {

        }


        protected override void Initialize()
        {
            base.Initialize();

            ClearColor = Color.Beige;
            DebugFontColor = Color.White;

            IsMouseVisible = true;

            // Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            _chessButtonCreator.Load(this);

            DebugFont = Content.Load<SpriteFont>("Fonts/debugFont");

            // Start from Main Menu Scene
            SceneManager.LoadScene<SceneMainMenu>();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DebugManager.Log($"[TEST LOG] Debug Mode: {DebugManager.DebugMode}");
            DebugManager.Log($"FPS: {(1.0f / gameTime.ElapsedGameTime.TotalSeconds).ToString("0.00")}");
            DebugManager.Log($"Mouse Position: {MonoGameEngine.Mouse.Position}");

            HandleKeyboardInputs();
        }

        private void HandleKeyboardInputs()
        {
            // Emergency exit on ESC key
            if (MonoGameEngine.Keyboard.IsKeyUp(Keys.Escape))
            {
                AppManager.StopApp();
            }
        }
    }
}
