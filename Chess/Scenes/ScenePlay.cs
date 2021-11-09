using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class ScenePlay : Scene
    {
        public GameManager GameManager { get; private set; }

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

            Button buttonSave = chessGameApp.CreateButton("Save Game", new Vector3(680, 190, uiLayerDepth));
            buttonSave.Enabled = false;
            Button buttonGiveUp = chessGameApp.CreateButton("Give Up", new Vector3(680, 250, uiLayerDepth));
            Button buttonPlayAgain = chessGameApp.CreateButton("Play Again", new Vector3(680, 310, uiLayerDepth));
            buttonPlayAgain.SetActive(false);
            Button buttonMainMenu = chessGameApp.CreateButton("Main Menu", new Vector3(680, 370, uiLayerDepth));
            buttonMainMenu.SetActive(false);

            GameManager = new GameManager(chessBoard.TileBoard);


            // Button Click Events
            buttonSave.OnLeftMouseUp += () =>
            {
                // TODO: <Saving code here>
            };
            buttonGiveUp.OnLeftMouseUp += () =>
            {
                GameManager.GiveUp(GameManager.PlayerTurn);
            };
            buttonPlayAgain.OnLeftMouseUp += () =>
            {
                buttonPlayAgain.SetActive(false);
                buttonMainMenu.SetActive(false);
                buttonSave.Visible = true;
                buttonGiveUp.SetActive(true);

                GameManager.CreateNewDefaultGame();
            };
            buttonMainMenu.OnLeftMouseUp += () =>
            {
                app.SceneManager.LoadScene<SceneMainMenu>();
            };


            // GameManager Events
            GameManager.OnGameStart += () =>
            {

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

                buttonSave.Visible = false;
                buttonGiveUp.SetActive(false);
                buttonPlayAgain.SetActive(true);
                buttonMainMenu.SetActive(true);
            };

            GameManager.OnPlayerTurnChange += (pieceTeam) =>
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

                turnCountLabel.Text = "Turn: " + (GameManager.TurnsTaken + 1);
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