using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameEngine;

namespace Chess
{
    class ChessGameManager : DrawableAppObject
    {
        // Tileboard
        private TileBoard _tileBoard;

        // Piece Collections
        private HashSet<Piece> _pieces = new HashSet<Piece>();
        private HashSet<Piece> _whitePieces = new HashSet<Piece>();
        private HashSet<Piece> _blackPieces = new HashSet<Piece>();

        // Kings
        private King _whiteKing;
        private King _blackKing;

        // Colors
        private Color CheckingPieceTint => Color.Red;

        // Moves Logic
        public MoveManager MoveManager { get; private set; }
        private Dictionary<Piece, Stack<Tile>> pieceToMoves = new Dictionary<Piece, Stack<Tile>>();
        private Dictionary<int, Piece> turnIndexToCapture = new Dictionary<int, Piece>();

        // Selected Piece Logic
        public Piece SelectedPiece { get; set; }
        public IEnumerable<Tile> SelectedPiecePossibleMoves { get; set; }

        // Player State Logic
        public ChessGameManagerStateMachine StateMachine { get; private set; }

        // Turn Logic
        public TurnManager TurnManager { get; private set; } = new TurnManager();

        // Save Logic
        public ChessSaveManager SaveManager { get; private set; }

        // Public Events
        public event Action OnGameStart;
        public event Action<Team?> OnGameFinished;
        public event Action OnGameUndoFinished;

        public ChessGameManager(TileBoard tileBoard)
        {
            _tileBoard = tileBoard;
            MoveManager = new MoveManager(this, _tileBoard);

            StateMachine = new ChessGameManagerStateMachine(this);
            SaveManager = new ChessSaveManager(this);
            
            // Subscribe functionality to all tiles in the board
            _tileBoard.CallFunctionOnAllTiles((tile) =>
            {
                tile.OnLeftMouseUp += () =>
                {
                    if (StateMachine.CurrentState is MovingPiece)
                    {
                        if (SelectedPiecePossibleMoves.Contains(tile))
                        {
                            PerformPlayerTurn(SelectedPiece, tile);
                        }
                    }
                };
            });
        }

        public void GiveUp(Team team)
        {
            switch (team)
            {
                case Team.White:
                    FinishGame(Team.Black);
                    break;
                case Team.Black:
                    FinishGame(Team.White);
                    break;
            }
        }

        public void ReturnFromGameFinished()
        {
            OnGameUndoFinished?.Invoke();
        }

        // In-Game Logic

        public IEnumerable<Tile> GetFilteredPossibleMoves(Piece piece)
        {
            IEnumerable<Tile> possibleMoves = piece.GetPossibleMoves(_tileBoard);

            var possibleMovesFilteredForCheck = new HashSet<Tile>(possibleMoves);

            // Filter out moves that would put in check
            foreach (Tile tile in possibleMoves)
            {
                Tile originalPositionOfPiece = piece.TilePosition;
                Piece originalPieceInTile = tile.Piece;

                piece.TilePosition = tile;

                switch (piece.Team)
                {
                    case Team.White:
                        if (_whiteKing.InCheck(_tileBoard))
                        {
                            possibleMovesFilteredForCheck.Remove(tile);
                        }

                        break;
                    case Team.Black:
                        if (_blackKing.InCheck(_tileBoard))
                        {
                            possibleMovesFilteredForCheck.Remove(tile);
                        }
                        break;
                }

                piece.TilePosition = originalPositionOfPiece;
                tile.Piece = originalPieceInTile;
            }

            // Filter out capturing the King
            var filteredPossibleMoves = possibleMovesFilteredForCheck.Where(tile => !(tile.Piece is King));

            return filteredPossibleMoves;
        }

        private void SelectPiece(Piece piece)
        {
            if (SelectedPiece != null)
            {
                DeselectSelectedPiece();
            }

            // Set selected piece pointer
            SelectedPiece = piece;

            // Update Player State
            StateMachine.SetState<MovingPiece>();
        }

        public void DeselectSelectedPiece()
        {
            if (SelectedPiece != null)
            {
                // Update Player State
                StateMachine.SetState<SelectingPiece>();
            }
        }

        private void RemovePieceFromGame(Piece piece)
        {
            piece.TilePosition.Piece = null;

            _pieces.Remove(piece);

            if (piece.Team == Team.White)
            {
                _whitePieces.Remove(piece);
            }
            else
            {
                _blackPieces.Remove(piece);
            }

            // Record the capture in the dictionary
            turnIndexToCapture[TurnManager.TurnIndex] = piece;
        }

        // Alias for RemovePieceFromGame
        private void Capture(Piece piece) => RemovePieceFromGame(piece);

        public void MovePieceTo(Piece piece, Tile targetTile)
        {
            // Check for captures
            if (targetTile.Piece != null && piece.Team != targetTile.Piece.Team)
            {
                Capture(targetTile.Piece);
            }

            // Record move in the dictionary to be able to backtrack later
            if (pieceToMoves.ContainsKey(piece))
            {
                pieceToMoves[piece].Push(piece.TilePosition);
            }
            else
            {
                // Lazy instantiate Stacks of moves for pieces as needed
                pieceToMoves[piece] = new Stack<Tile>();
                pieceToMoves[piece].Push(piece.TilePosition);
            }

            // Move the piece
            piece.MoveTo(targetTile);
        }

        public void MovePieceBack(Piece piece)
        {
            // If this piece made moves
            if (pieceToMoves[piece].Count > 0)
            {
                // Move it back one move
                Tile previousTile = pieceToMoves[piece].Pop();
                piece.MoveTo(previousTile);
                
                // If this was the first move
                if (pieceToMoves[piece].Count <= 0)
                {
                    // Set the piece to unmoved
                    piece.Unmoved = true;

                    // Remove the key from the dictionary
                    pieceToMoves.Remove(piece);
                }

                // Check if that turn any captures were made
                if (turnIndexToCapture.ContainsKey(TurnManager.TurnIndex - 1))
                {
                    Piece capturedPiece = turnIndexToCapture[TurnManager.TurnIndex - 1];

                    // Bring back piece captured to the game
                    AddPieceToGame(capturedPiece);

                    // Set the Piece in that tile to the piece that was captured
                    capturedPiece.TilePosition.Piece = capturedPiece;

                    // Remove it from the dictionary
                    turnIndexToCapture.Remove(TurnManager.TurnIndex - 1);
                }
            }
            
        }

        private void FinishGame(Team? team)
        {
            StateMachine.SetState<WaitingOnFinishedGame>();
            OnGameFinished?.Invoke(team);
        }

        /// <summary>
        /// Checks if the King has been checked and colors the checking pieces.
        /// Also checks if a player has won and calls FinishGame.
        /// </summary>
        public void CheckGameState()
        {
            // Determine if there has been a check/checkmate/tie
            switch (TurnManager.TurnTeam)
            {
                case Team.White:
                    {
                        List<Tile> whitePossibleMoves = new List<Tile>();

                        foreach (Piece piece in _whitePieces)
                        {
                            whitePossibleMoves.AddRange(GetFilteredPossibleMoves(piece));
                        }

                        // Black checked white 
                        if (_whiteKing.InCheck(_tileBoard, out IEnumerable<Piece> checkingPieces))
                        {
                            // Color pieces that checked the king
                            foreach (Piece checkingPiece in checkingPieces)
                            {
                                checkingPiece.TilePosition.Tint = CheckingPieceTint;
                            }

                            // Black checkmated white (BLACK WINS)
                            if (whitePossibleMoves.Count <= 0)
                            {
                                FinishGame(Team.Black);
                            }
                        }
                        else
                        {
                            // White has no possible moves and is not in check (TIE)
                            if (whitePossibleMoves.Count <= 0)
                            {
                                FinishGame(null);
                            }
                        }
                    }
                    
                    break;
                case Team.Black:
                    {
                        List<Tile> blackPossibleMoves = new List<Tile>();

                        foreach (Piece piece in _blackPieces)
                        {
                            blackPossibleMoves.AddRange(GetFilteredPossibleMoves(piece));
                        }

                        // White checked black 
                        if (_blackKing.InCheck(_tileBoard, out IEnumerable<Piece> checkingPieces))
                        {
                            // Color pieces that checked the king
                            foreach (Piece checkingPiece in checkingPieces)
                            {
                                checkingPiece.TilePosition.Tint = CheckingPieceTint;
                            }

                            // White checkmated black (WHITE WINS)
                            if (blackPossibleMoves.Count <= 0)
                            {
                                FinishGame(Team.White);
                            }
                        }
                        else
                        {
                            // Black has no possible moves and is not in check (TIE)
                            if (blackPossibleMoves.Count <= 0)
                            {
                                FinishGame(null);
                            }
                        }
                    }
                    break;
            }

            // Check if any other form of tie has occurred

            // Helper function to determine whether a collection contains any of this type of object
            bool ContainsPieceOfType<T>(IEnumerable<Piece> pieces) => pieces.OfType<T>().Any();

            // King vs King (TIE)
            if (_whitePieces.Count == 1 && ContainsPieceOfType<King>(_whitePieces) && _blackPieces.Count == 1 && ContainsPieceOfType<King>(_blackPieces))
            {
                FinishGame(null);
            }

            // Draw if king and bishop vs king
            if ((_whitePieces.Count == 2 && _blackPieces.Count == 1 && ContainsPieceOfType<King>(_whitePieces) && ContainsPieceOfType<Bishop>(_whitePieces) && ContainsPieceOfType<King>(_blackPieces)) ||
                (_blackPieces.Count == 2 && _whitePieces.Count == 1 && ContainsPieceOfType<King>(_blackPieces) && ContainsPieceOfType<Bishop>(_blackPieces) && ContainsPieceOfType<King>(_whitePieces)))
            {
                FinishGame(null);
            }

            // Draw if king and knight vs king
            if ((_whitePieces.Count == 2 && _blackPieces.Count == 1 && ContainsPieceOfType<King>(_whitePieces) && ContainsPieceOfType<Knight>(_whitePieces) && ContainsPieceOfType<King>(_blackPieces)) ||
                (_blackPieces.Count == 2 && _whitePieces.Count == 1 && ContainsPieceOfType<King>(_blackPieces) && ContainsPieceOfType<Knight>(_blackPieces) && ContainsPieceOfType<King>(_whitePieces)))
            {
                FinishGame(null);
            }

            // Draw if king and bishop vs king and bishop on the same color
            if (_whitePieces.Count == 2 && _blackPieces.Count == 2 && ContainsPieceOfType<King>(_whitePieces) && ContainsPieceOfType<Bishop>(_whitePieces) && ContainsPieceOfType<King>(_blackPieces) && ContainsPieceOfType<Bishop>(_blackPieces) &&
                    _whitePieces.First(piece => piece is Bishop).TilePosition.Coordinate.X % 2 == _blackPieces.First(piece => piece is Bishop).TilePosition.Coordinate.X % 2)
            {
                FinishGame(null);
            }
        }
        
        public void PerformPlayerTurn(Piece piece, Tile targetTile)
        {
            Tile.TileName from = piece.TilePosition.Name;
            Tile.TileName to = targetTile.Name;

            MoveManager.Do(new Move(SelectedPiece, from, to));
        }


        // Game Start Logic

        private void StartGame()
        {
            _pieces.Clear();
            _whitePieces.Clear();
            _blackPieces.Clear();

            _tileBoard.ClearPiecesOnTiles();
            _tileBoard.ClearTilesTint();

            MoveManager.Clear();

            TurnManager.SetTurn(0, Team.White);

            StateMachine.SetState<SelectingPiece>();

            OnGameStart?.Invoke();
        }

        // Piece builder/factory function
        private Piece CreatePiece<T>(Tile position, Team team) where T : Piece, new()
        {
            Piece piece = new T();
            piece.TilePosition = position;
            piece.Transform.Position = piece.TilePosition.Transform.GlobalPosition;
            piece.Team = team;

            // Events
            piece.OnLeftMouseUp += () =>
            {
                switch (StateMachine.CurrentState)
                {
                    case SelectingPiece state:
                        if (piece.Team == TurnManager.TurnTeam)
                            SelectPiece(piece);
                        break;
                    case MovingPiece state:
                        if (piece == SelectedPiece)
                            DeselectSelectedPiece();
                        else if (piece.Team == TurnManager.TurnTeam)
                        {
                            DeselectSelectedPiece();
                            SelectPiece(piece);
                        }
                        break;
                    default:
                        // Do Nothing
                        break;
                }
            };

            if (piece is Pawn)
            {
                (piece as Pawn).OnEnPassantCapture += pieceToCapture =>
                {
                    Capture(pieceToCapture);
                };
            }

            if (piece is King)
            {
                (piece as King).OnCastle += (rook, rookDestination) =>
                {
                    rook.MoveTo(rookDestination);
                };
            }

            return piece;
        }

        // Adds piece to the correct collections
        private void AddPieceToGame(Piece piece)
        {
            _pieces.Add(piece);

            if (piece.Team == Team.White)
            {
                _whitePieces.Add(piece);
            }
            else
            {
                _blackPieces.Add(piece);
            }
        }

        /// <summary>
        /// Starts new game with the traditional Chess piece set up
        /// </summary>
        public void StartNewGame()
        {
            StartGame();

            // White
            _whiteKing = CreatePiece<King>(_tileBoard[3, 0], Team.White) as King;
            AddPieceToGame(_whiteKing);

            AddPieceToGame(CreatePiece<Bishop>(_tileBoard[2, 0], Team.White));
            AddPieceToGame(CreatePiece<Bishop>(_tileBoard[5, 0], Team.White));
            AddPieceToGame(CreatePiece<Knight>(_tileBoard[1, 0], Team.White));
            AddPieceToGame(CreatePiece<Knight>(_tileBoard[6, 0], Team.White));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[0, 1], Team.White));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[1, 1], Team.White));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[2, 1], Team.White));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[3, 1], Team.White));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[4, 1], Team.White));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[5, 1], Team.White));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[6, 1], Team.White));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[7, 1], Team.White));
            AddPieceToGame(CreatePiece<Queen>(_tileBoard[4, 0], Team.White));
            AddPieceToGame(CreatePiece<Rook>(_tileBoard[0, 0], Team.White));
            AddPieceToGame(CreatePiece<Rook>(_tileBoard[7, 0], Team.White));

            // Black
            _blackKing = CreatePiece<King>(_tileBoard[3, 7], Team.Black) as King;
            AddPieceToGame(_blackKing);

            AddPieceToGame(CreatePiece<Bishop>(_tileBoard[2, 7], Team.Black));
            AddPieceToGame(CreatePiece<Bishop>(_tileBoard[5, 7], Team.Black));
            AddPieceToGame(CreatePiece<Knight>(_tileBoard[1, 7], Team.Black));
            AddPieceToGame(CreatePiece<Knight>(_tileBoard[6, 7], Team.Black));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[0, 6], Team.Black));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[1, 6], Team.Black));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[2, 6], Team.Black));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[3, 6], Team.Black));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[4, 6], Team.Black));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[5, 6], Team.Black));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[6, 6], Team.Black));
            AddPieceToGame(CreatePiece<Pawn>(_tileBoard[7, 6], Team.Black));
            AddPieceToGame(CreatePiece<Queen>(_tileBoard[4, 7], Team.Black));
            AddPieceToGame(CreatePiece<Rook>(_tileBoard[0, 7], Team.Black));
            AddPieceToGame(CreatePiece<Rook>(_tileBoard[7, 7], Team.Black));

            // Load all Pieces
            foreach (Piece piece in _pieces)
            {
                piece.Load(AppManager.AppRunning);
            }
        }

        public void LoadGameFromFile(string path)
        {
            if (File.Exists(path))
            {
                // Read in data
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                ChessSaveData data = formatter.Deserialize(stream) as ChessSaveData;
                stream.Close();

                // Start new game and make all the moves in the save file immediately
                StartNewGame();
                for(int i = 0; i < data.movesCount; i++)
                {
                    int[] fromCoordinate = data.fromCoordinates[i];
                    int[] toCoordinate = data.toCoordinates[i];

                    Piece piece = _tileBoard[fromCoordinate[0], fromCoordinate[1]].Piece;
                    Tile tile = _tileBoard[toCoordinate[0], toCoordinate[1]];

                    PerformPlayerTurn(piece, tile);
                }
            }
        }


        // Update & Draw

        protected override void OnUpdate(GameTime gameTime)
        {
            HandlePlayerInputs();

            StateMachine.Update(gameTime);

            foreach (Piece piece in _pieces)
            {
                piece.Update(gameTime);
            }
        }

        private void HandlePlayerInputs()
        {
            if (Input.IsKeyCombinationDown(Keys.LeftControl, Keys.Z))
            {
                MoveManager.Undo();
            }

            if (Input.IsKeyCombinationDown(Keys.LeftControl, Keys.Y))
            {
                MoveManager.Redo();
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            foreach (Piece piece in _pieces)
            {
                piece.Draw(spriteBatch);
            }
        }
    }
}
