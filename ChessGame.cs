using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

using Chess.Managers;
using Chess.UI;

namespace Chess
{
    public class ChessGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        UIData uiData;

        #region Managers
        GameManager gameManager;
        SceneManager sceneManager;
        UIManager uiManager;
        FileManager fileManager;
        #endregion

        #region Assets
        Dictionary<FontType, SpriteFont> fonts;

        #region Textures
        Texture2D titleImage;
        Texture2D menuBackgroundTexture;
        Texture2D floorBackground;
        Texture2D brownTileTexture;
        Texture2D whiteTileTexture;
        Texture2D boardEdge;

        Dictionary<UITextureType, Texture2D> uiTextures;
        Texture2D buttonTexture;

        Dictionary<Tuple<PieceType, Team>, Texture2D> pieceTextures;
        Texture2D blackBishop;
        Texture2D blackKnight;
        Texture2D blackKing;
        Texture2D blackPawn;
        Texture2D blackQueen;
        Texture2D blackRook;
        Texture2D whiteBishop;
        Texture2D whiteKnight;
        Texture2D whiteKing;
        Texture2D whitePawn;
        Texture2D whiteQueen;
        Texture2D whiteRook;
        #endregion
        #endregion

        #region GameObjects
        MenuBackground menuBackground;

        #region Menu Items
        Button buttonPlay;
        Button buttonLoad;
        Button buttonQuit;
        #endregion

        #region Game Items
        ChessBoard board;
        StringObject turnText;
        Button buttonGiveUp;
        Button buttonSave;
        #endregion

        #region End Items
        StringObject winnerText;
        Button buttonReturn;
        #endregion
        #endregion

        #region Scenes
        List<ObjectContainer> scenes;
        ObjectContainer menuScene;
        ObjectContainer gameScene;
        ObjectContainer endScene;
        #endregion

        public ChessGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            //Window.AllowUserResizing = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Textures
            titleImage = Content.Load<Texture2D>("ChessTitle");
            menuBackgroundTexture = Content.Load<Texture2D>("chessBoard");
            brownTileTexture = Content.Load<Texture2D>("brownTile");
            whiteTileTexture = Content.Load<Texture2D>("whiteTile");
            boardEdge = Content.Load<Texture2D>("boardEdge");
            floorBackground = Content.Load<Texture2D>("floorBackground");

            buttonTexture = Content.Load<Texture2D>("buttonTexture");
            uiTextures = new Dictionary<UITextureType, Texture2D>()
            {
                { UITextureType.Button, buttonTexture}
            };

            menuBackground = new MenuBackground(menuBackgroundTexture, Rectangle.Empty);

            #region Pieces
            blackBishop = Content.Load<Texture2D>("Pieces/bb");
            blackKnight = Content.Load<Texture2D>("Pieces/bh");
            blackKing = Content.Load<Texture2D>("Pieces/bk");
            blackPawn = Content.Load<Texture2D>("Pieces/bp");
            blackQueen = Content.Load<Texture2D>("Pieces/bq");
            blackRook = Content.Load<Texture2D>("Pieces/br");
            whiteBishop = Content.Load<Texture2D>("Pieces/wb");
            whiteKnight = Content.Load<Texture2D>("Pieces/wh");
            whiteKing = Content.Load<Texture2D>("Pieces/wk");
            whitePawn = Content.Load<Texture2D>("Pieces/wp");
            whiteQueen = Content.Load<Texture2D>("Pieces/wq");
            whiteRook = Content.Load<Texture2D>("Pieces/wr");

            pieceTextures = new Dictionary<Tuple<PieceType, Team>, Texture2D>()
            {
                { Tuple.Create(PieceType.Bishop, Team.Black), blackBishop },
                { Tuple.Create(PieceType.Knight, Team.Black), blackKnight },
                { Tuple.Create(PieceType.King, Team.Black), blackKing },
                { Tuple.Create(PieceType.Pawn, Team.Black), blackPawn },
                { Tuple.Create(PieceType.Queen, Team.Black), blackQueen },
                { Tuple.Create(PieceType.Rook, Team.Black), blackRook },
                { Tuple.Create(PieceType.Bishop, Team.White), whiteBishop },
                { Tuple.Create(PieceType.Knight, Team.White), whiteKnight },
                { Tuple.Create(PieceType.King, Team.White), whiteKing },
                { Tuple.Create(PieceType.Pawn, Team.White), whitePawn },
                { Tuple.Create(PieceType.Queen, Team.White), whiteQueen },
                { Tuple.Create(PieceType.Rook, Team.White), whiteRook },
            };
            #endregion

            #endregion

            #region Fonts
            fonts = new Dictionary<FontType, SpriteFont>()
            {
                { FontType.Debug, Content.Load<SpriteFont>("Fonts/debugFont") },
                { FontType.Button, Content.Load<SpriteFont>("Fonts/buttonFont") },
                { FontType.Win, Content.Load<SpriteFont>("Fonts/winFont") }
            };
            #endregion

            #region Text
            turnText = new StringObject("White's turn", fonts[FontType.Button], new Vector2(350, 20));
            winnerText = new StringObject(" wins!", fonts[FontType.Win], new Vector2(200, 200));
            #endregion

            board = new ChessBoard(boardEdge, new Rectangle(159, 62, boardEdge.Width, boardEdge.Height), brownTileTexture, whiteTileTexture);
            uiData = new UIData(spriteBatch, fonts, uiTextures);
            uiManager = new UIManager(uiData);

            #region Buttons
            buttonPlay = uiManager.CreateButton(new Rectangle(350, 300, buttonTexture.Width, buttonTexture.Height), "Play", () =>
            {
                gameManager.CreateNewDefaultGame();
                gameManager.StartGame(gameScene);
                sceneManager.TransitionToScene(gameScene);
                AppManager.AppState = AppState.Game;
            });
            buttonQuit = uiManager.CreateButton(new Rectangle(350, 400, buttonTexture.Width, buttonTexture.Height), "Quit", () =>
            {
                this.Exit();
            });
            buttonReturn = uiManager.CreateButton(new Rectangle(350, 300, buttonTexture.Width, buttonTexture.Height), "Main Menu", () =>
            {
                sceneManager.TransitionToScene(menuScene);
                AppManager.AppState = AppState.MainMenu;
            });
            buttonGiveUp = uiManager.CreateButton(new Rectangle(650, 250, buttonTexture.Width, buttonTexture.Height), "Give up", () =>
            {
                switch (gameManager.PlayerTurn)
                {
                    case Team.White:
                        gameManager.OnGameFinished?.Invoke(Team.Black);
                        break;
                    case Team.Black:
                        gameManager.OnGameFinished?.Invoke(Team.White);
                        break;
                }
            });
            buttonSave = uiManager.CreateButton(new Rectangle(650, 300, buttonTexture.Width, buttonTexture.Height), "Save Game", () =>
            {
                // <Saving code here>
            });
            buttonLoad = uiManager.CreateButton(new Rectangle(350, 350, buttonTexture.Width, buttonTexture.Height), "Load Game", () =>
            {
                // <Loading code here>
            });
            #endregion

            #region Scenes
            scenes = new List<ObjectContainer>();

            menuScene = new ObjectContainer();
            menuScene.AddChild(menuBackground,
                               new GameObject(titleImage, new Rectangle(300, 150, titleImage.Width, titleImage.Height)),
                               buttonPlay,
                               buttonQuit,
                               buttonLoad
                               );
            scenes.Add(menuScene);

            gameScene = new ObjectContainer();
            gameScene.AddChild(new GameObject(floorBackground, new Rectangle(0, 0, floorBackground.Width, floorBackground.Height)),
                               board,
                               turnText,
                               buttonGiveUp,
                               buttonSave
                               );
            scenes.Add(gameScene);

            endScene = new ObjectContainer();
            endScene.AddChild(menuBackground,
                              winnerText,
                              buttonReturn);
            scenes.Add(endScene);
            #endregion

            gameManager = new GameManager(board, pieceTextures);
            sceneManager = new SceneManager(scenes);

            #region Events
            gameManager.OnPlayerTurnChange += () =>
            {
                switch (gameManager.PlayerTurn)
                {
                    case Team.White:
                        turnText.Text = "White's turn";
                        break;
                    case Team.Black:
                        turnText.Text = "Black's turn";
                        break;
                }
            };

            gameManager.OnGameFinished += winner =>
            {
                switch (winner)
                {
                    case Team.White:
                        winnerText.Text = "White wins!";
                        break;
                    case Team.Black:
                        winnerText.Text = "Black wins!";
                        break;
                    case null:
                        winnerText.Text = "Tie game";
                        break;
                }

                gameManager.EndGame(gameScene);
                sceneManager.TransitionToScene(endScene);
                AppManager.AppState = AppState.End;
            };
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            HandleMouseInputs();
            HandleKeyboardInputs();
            sceneManager.CurrentScene.Update(gameTime);
            LogDebugs();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            spriteBatch.Begin();
            sceneManager.CurrentScene.Draw(spriteBatch);
            uiManager.DrawDebug();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        #region Helper Methods
        private void HandleMouseInputs()
        {
            if (AppManager.RightMouseDown && gameManager.PlayerState == PlayerState.Moving)
                gameManager.OnPieceDeselected?.Invoke();
        }

        private void HandleKeyboardInputs()
        {
            AppManager.UpdateInputStates();

            // Emergency Exit
            if (AppManager.SingleKeyPress(Keys.Escape))
                this.Exit();

            // Debug Logic
            if (AppManager.SingleKeyPress(Keys.OemTilde))
                AppManager.ToggleDebug();
        }

        // To aid development.
        // Use uiManager's LogDebugLine(string) method to log debug calls every update.
        private void LogDebugs()
        {
            if (AppManager.DebugMode)
            {
                uiManager.LogDebugLine($"Mouse Position: ({AppManager.MouseState.X}, {AppManager.MouseState.Y})");
            }
        }
        #endregion
    }
}
