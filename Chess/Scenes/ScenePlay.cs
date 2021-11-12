using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class ScenePlay : Scene
    {
        public ChessGameManager GameManager { get; private set; }

        protected override void OnSceneLoad(MonoGameApp app)
        {
            const float backgroundLayerDepth = 0.010f;
            const float boardLayerDepth = 0.050f;
            const float uiLayerDepth = 0.090f;

            ChessGame chessGameApp = app as ChessGame;

            // Textures
            Texture2D floorBackgroundTexture = app.Content.Load<Texture2D>("floorBackground");


            // Fonts
            SpriteFont textFont = app.Content.Load<SpriteFont>("Fonts/textFont");


            // GameObjects
            TexturedGameObject floorBackgroundGameObject = new TexturedGameObject();
            floorBackgroundGameObject.Texture = floorBackgroundTexture;
            floorBackgroundGameObject.Transform.Position = new Vector3(0, 0, backgroundLayerDepth);

            TextGameObject turnCountLabel = new TextGameObject();
            turnCountLabel.Font = textFont;
            turnCountLabel.Text = "Turn: 1";
            turnCountLabel.Transform.Position = new Vector3(320, 12, uiLayerDepth);
            turnCountLabel.Transform.Scale = new Vector2(0.36f, 0.36f);

            TextGameObject statusLabel = new TextGameObject();
            statusLabel.Font = textFont;
            statusLabel.Text = "White's turn";
            statusLabel.Transform.Position = new Vector3(320, 42, uiLayerDepth);
            statusLabel.Transform.Scale = new Vector2(0.36f, 0.36f);

            ChessBoard chessBoard = new ChessBoard();
            chessBoard.Transform.Position = new Vector3(166, 80, boardLayerDepth);

            MonoGameEngine.Button buttonSave = chessGameApp.CreateButton("Save Game", new Vector3(680, 190, uiLayerDepth));
            MonoGameEngine.Button buttonGiveUp = chessGameApp.CreateButton("Give Up", new Vector3(680, 250, uiLayerDepth));
            MonoGameEngine.Button buttonPlayAgain = chessGameApp.CreateButton("Play Again", new Vector3(680, 310, uiLayerDepth));
            buttonPlayAgain.SetActive(false);
            MonoGameEngine.Button buttonMainMenu = chessGameApp.CreateButton("Main Menu", new Vector3(680, 370, uiLayerDepth));
            buttonMainMenu.SetActive(false);

            GameManager = new ChessGameManager(chessBoard.TileBoard);


            // Button Click Events
            buttonSave.OnLeftMouseUp += () =>
            {
                // Open Dialog Box to select save file location
                using (SaveFileDialog openFileDialog = new SaveFileDialog())
                {
                    string relativePathToSavedGames = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "./SavedGames"));
                    openFileDialog.InitialDirectory = relativePathToSavedGames;
                    openFileDialog.Filter = "Save files (*.sav)|*.sav";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Path of selected file
                        string filePath = openFileDialog.FileName;

                        // Save the game at selected filepath
                        GameManager.SaveManager.Save(filePath);
                    }
                }
            };
            buttonGiveUp.OnLeftMouseUp += () =>
            {
                GameManager.GiveUp(GameManager.TurnManager.TurnTeam);
            };
            buttonPlayAgain.OnLeftMouseUp += () =>
            {
                GameManager.StartNewGame();
            };
            buttonMainMenu.OnLeftMouseUp += () =>
            {
                app.SceneManager.LoadScene<SceneMainMenu>();
            };


            // GameManager Events
            GameManager.OnGameStart += () =>
            {
                buttonGiveUp.Enabled = true;
                buttonPlayAgain.SetActive(false);
                buttonMainMenu.SetActive(false);
            };

            GameManager.OnGameUndoFinished += () =>
            {
                buttonGiveUp.Enabled = true;
                buttonPlayAgain.SetActive(false);
                buttonMainMenu.SetActive(false);
            };

            GameManager.OnGameFinished += winner =>
            {
                // Set winner text and switch buttons
                switch (winner)
                {
                    case Team.White:
                        {
                            statusLabel.Text = "White wins!";
                        }
                        break;
                    case Team.Black:
                        {
                            statusLabel.Text = "Black wins!";
                        }
                        break;
                    default:
                        {
                            statusLabel.Text = "Tie game";
                        }
                        break;
                }

                buttonGiveUp.Enabled = false;
                buttonPlayAgain.SetActive(true);
                buttonMainMenu.SetActive(true);
            };

            GameManager.TurnManager.OnTurnChange += (pieceTeam) =>
            {
                switch (pieceTeam)
                {
                    case Team.White:
                        {
                            statusLabel.Text = "White's turn";
                            break;
                        }
                    case Team.Black:
                        {
                            statusLabel.Text = "Black's turn";
                            break;
                        }
                }

                turnCountLabel.Text = "Turn: " + (GameManager.TurnManager.TurnIndex + 1);
            };


            // Add Calls
            AddObjects(floorBackgroundGameObject);
            AddObjects(turnCountLabel);
            AddObjects(statusLabel);
            AddObjects(chessBoard);
            AddObjects(buttonSave);
            AddObjects(buttonGiveUp);
            AddObjects(buttonPlayAgain);
            AddObjects(buttonMainMenu);
            AddObjects(GameManager);
        }
    }
}