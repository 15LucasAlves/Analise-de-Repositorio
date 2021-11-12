using System.IO;
using System.Windows.Forms;
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

            MonoGameEngine.Button buttonPlay = chessGameApp.CreateButton("Play", new Vector3(350, 300, uiLayerDepth));
            MonoGameEngine.Button buttonLoad = chessGameApp.CreateButton("Load", new Vector3(350, 350, uiLayerDepth));
            MonoGameEngine.Button buttonQuit = chessGameApp.CreateButton("Quit", new Vector3(350, 400, uiLayerDepth));

            // Button Click Events
            buttonPlay.OnLeftMouseUp += () =>
                {
                    app.SceneManager.LoadScene<ScenePlay>();
                    ScenePlay playSceneCasted = app.SceneManager.CurrentScene as ScenePlay;
                    playSceneCasted.GameManager.StartNewGame();
                };

            buttonLoad.OnLeftMouseUp += () =>
                {
                    // Open Dialog Box to select Save File
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        string relativePathToSavedGames = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "./SavedGames"));
                        openFileDialog.InitialDirectory = relativePathToSavedGames;
                        openFileDialog.Filter = "Save files (*.sav)|*.sav";
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Path of selected file
                            string filePath = openFileDialog.FileName;

                            // Start game from filepath
                            app.SceneManager.LoadScene<ScenePlay>();
                            ScenePlay playSceneCasted = app.SceneManager.CurrentScene as ScenePlay;
                            playSceneCasted.GameManager.LoadGameFromFile(filePath);
                        }
                    }
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
