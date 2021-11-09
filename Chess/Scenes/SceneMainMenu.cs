using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class SceneMainMenu : Scene
    {
        protected override void OnSceneLoad(MonoGameApp app)
        {
            const float backgroundLayerDepth = 0.010f;
            const float uiLayerDepth = 0.090f;

            ChessGame chessGameApp = app as ChessGame;

            // Textures
            Texture2D titleTexture = app.Content.Load<Texture2D>("chessTitle");
            Texture2D backgroundTexture = app.Content.Load<Texture2D>("chessBoard");

            // GameObjects
            MainMenuBackground[] backgrounds = new MainMenuBackground[4];
            backgrounds[0] = new MainMenuBackground();
            backgrounds[0].Transform.Position = new Vector3(0, 0, backgroundLayerDepth);
            backgrounds[1] = new MainMenuBackground();
            backgrounds[1].Transform.Position = new Vector3(-backgroundTexture.Width, 0, backgroundLayerDepth);
            backgrounds[2] = new MainMenuBackground();
            backgrounds[2].Transform.Position = new Vector3(0, backgroundTexture.Height, backgroundLayerDepth);
            backgrounds[3] = new MainMenuBackground();
            backgrounds[3].Transform.Position = new Vector3(-backgroundTexture.Width, backgroundTexture.Height, backgroundLayerDepth);

            TexturedGameObject titleGameObject = new TexturedGameObject();
            titleGameObject.Texture = titleTexture;
            titleGameObject.Transform.Position = new Vector3(300, 150, uiLayerDepth);

            Button buttonPlay = chessGameApp.CreateButton("Play", new Vector3(350, 300, uiLayerDepth));
            Button buttonLoad = chessGameApp.CreateButton("Load", new Vector3(350, 350, uiLayerDepth));
            buttonLoad.Enabled = false;
            Button buttonQuit = chessGameApp.CreateButton("Quit", new Vector3(350, 400, uiLayerDepth));

            // Button Click Events
            buttonPlay.OnLeftMouseUp += () =>
                {
                    app.SceneManager.LoadScene<ScenePlay>();
                    ScenePlay playSceneCasted = app.SceneManager.CurrentScene as ScenePlay;
                    playSceneCasted.GameManager.CreateNewDefaultGame();
                };
            buttonLoad.OnLeftMouseUp += () =>
                {
                // TODO: <Loading code here>
            };
            buttonQuit.OnLeftMouseUp += () =>
            {
                AppManager.StopApp();
            };

            // Add Calls
            AddObjects(backgrounds);
            AddObjects(titleGameObject);
            AddObjects(buttonPlay);
            AddObjects(buttonLoad);
            AddObjects(buttonQuit);
        }
    }
}
