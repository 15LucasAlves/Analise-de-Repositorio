using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameEngine;

namespace Chess
{
    class GameManager : DrawableAppObject
    {
        // Tileboards
        private TileBoard _tileBoard;

        // Private Events
        private event Action<Piece> OnPieceSelected;
        private event Action OnPieceDeselected;
        private event Action<Piece, Tile> OnMoveSelected;
        private event Action<IEnumerable<Piece>> OnKingChecked;

        // Pieces
        private HashSet<Piece> _pieces = new HashSet<Piece>();
        private HashSet<Piece> _whitePieces = new HashSet<Piece>();
        private HashSet<Piece> _blackPieces = new HashSet<Piece>();

        private King _whiteKing;
        private King _blackKing;

        // Selected Piece Logic
        private Piece _selectedPiece;
        private IEnumerable<Tile> _selectedPiecePossibleMoves;

        // Move Logic
        private MoveManager _moveManager = new MoveManager();

        // Colors
        private Color _selectedPieceTint = Color.LightSkyBlue;
        private Color _selectedPiecePossibleMovesTint = Color.Yellow;
        private Color _checkingPieceTint = Color.Red;

        // Player State
        private enum PlayerState { SelectingPiece, MovingPiece, FinishedGame }
        private PlayerState _playerState;

        // Turn
        public Team PlayerTurn { get; private set; }
        public int TurnsTaken { get; private set; }

        // Public Events
        public Action<Team> OnPlayerTurnChange;
        public event Action OnGameStart;
        public event Action<Team?> OnGameFinished;

        public GameManager(TileBoard tileBoard)
        {
            _tileBoard = tileBoard;

            // Events
            OnPieceSelected = piece =>
            {
                // Set selected piece pointer
                _selectedPiece = piece;
                // Get its possible moves
                _selectedPiecePossibleMoves = GetFilteredPossibleMoves(_selectedPiece);

                // Tint everything properly
                _selectedPiece.TilePosition.Tint = _selectedPieceTint;
                foreach (Tile tile in _selectedPiecePossibleMoves)
                {
                    tile.Tint = _selectedPiecePossibleMovesTint;
                }

                // Update Player State
                _playerState = PlayerState.MovingPiece;
            };

            OnPieceDeselected = () =>
            {
                // Clear tints
                _selectedPiece.TilePosition.Tint = Color.White;
                foreach (Tile tile in _selectedPiecePossibleMoves)
                {
                    tile.Tint = Color.White;
                }

                // Clear selected piece pointer and its possible moves
                _selectedPiece = null;
                _selectedPiecePossibleMoves = null;

                // Update Player State
                _playerState = PlayerState.SelectingPiece;
            };

            OnMoveSelected = (piece, tile) =>
            {
                if (_playerState == PlayerState.MovingPiece)
                {
                    if (_selectedPiecePossibleMoves.Contains(tile))
                    {
                        OnPieceDeselected?.Invoke();

                        MovePieceTo(piece, tile);

                        TogglePlayerTurn();

                        _tileBoard.ClearTilesTint();

                        CheckGameFinish();
                    }
                }
            };

            OnKingChecked += checkingPieces =>
            {
                foreach (Piece checkingPiece in checkingPieces)
                {
                    checkingPiece.TilePosition.Tint = _checkingPieceTint;
                }
            };

            _tileBoard.ApplyFunctionToAllTiles((tile) =>
            {
                tile.OnLeftMouseUp += () =>
                {
                    if (_playerState == PlayerState.MovingPiece)
                    {
                        OnMoveSelected?.Invoke(_selectedPiece, tile);
                    }
                };
            });
        }

        // Helper function
        private bool ContainsPieceOfType<T>(IEnumerable<Piece> pieces) where T : Piece
        {
            return pieces.OfType<T>().Any();
        }

        // Invokes OnGameFinished event with the correct winner (if conditions have been met).
        private void CheckGameFinish()
        {
            List<Tile> allPossiblePlayerMoves = new List<Tile>();

            IEnumerable<Piece> checkingPieces;

            // Black chekmated white (BLACK WINS)
            if (_whiteKing.InCheck(_tileBoard, out checkingPieces))
            {
                OnKingChecked?.Invoke(checkingPieces);

                foreach (Piece piece in _whitePieces)
                    allPossiblePlayerMoves.AddRange(GetFilteredPossibleMoves(piece));


                if (allPossiblePlayerMoves.Count <= 0)
                {
                    FinishGame(Team.Black);
                }
            }
            // White checkmated black (WHITE WINS)
            else if (_blackKing.InCheck(_tileBoard, out checkingPieces))
            {
                OnKingChecked?.Invoke(checkingPieces);

                foreach (Piece piece in _blackPieces)
                    allPossiblePlayerMoves.AddRange(GetFilteredPossibleMoves(piece));

                if (allPossiblePlayerMoves.Count <= 0)
                {
                    FinishGame(Team.White);
                }
            }
            else
            {
                // White has no possible moves and is not in check (TIE)
                foreach (Piece piece in _whitePieces)
                    allPossiblePlayerMoves.AddRange(GetFilteredPossibleMoves(piece));

                if (allPossiblePlayerMoves.Count <= 0)
                {
                    FinishGame(null);
                }

                allPossiblePlayerMoves.Clear();

                // Black has no possible moves and is not in check (TIE)
                foreach (Piece piece in _blackPieces)
                    allPossiblePlayerMoves.AddRange(GetFilteredPossibleMoves(piece));

                if (allPossiblePlayerMoves.Count <= 0)
                {
                    FinishGame(null);
                }
            }

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

        private void TogglePlayerTurn()
        {
            switch (PlayerTurn)
            {
                case Team.White:
                    {
                        PlayerTurn = Team.Black;
                        break;
                    }
                case Team.Black:
                    {
                        PlayerTurn = Team.White;
                        break;
                    }
            }

            TurnsTaken++;
            OnPlayerTurnChange?.Invoke(PlayerTurn);
        }

        private void SetTurn(int turnIndex, Team team)
        {
            TurnsTaken = turnIndex;
            PlayerTurn = team;
            OnPlayerTurnChange?.Invoke(PlayerTurn);
        }

        private IEnumerable<Tile> GetFilteredPossibleMoves(Piece piece)
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

        private void RestartGame()
        {
            _pieces.Clear();
            _whitePieces.Clear();
            _blackPieces.Clear();
            // _moves.Clear();

            _tileBoard.ClearPiecesOnTiles();
            _tileBoard.ClearTilesTint();

            SetTurn(0, Team.White);

            _playerState = PlayerState.SelectingPiece;

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
                switch (_playerState)
                {
                    case PlayerState.SelectingPiece:
                        if (piece.Team == PlayerTurn)
                            OnPieceSelected?.Invoke(piece);
                        break;
                    case PlayerState.MovingPiece:
                        if (piece == _selectedPiece)
                            OnPieceDeselected?.Invoke();
                        else if (piece.Team == PlayerTurn)
                        {
                            OnPieceDeselected?.Invoke();
                            OnPieceSelected?.Invoke(piece);
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
        }

        // Alias for RemovePieceFromGame
        private void Capture(Piece piece) => RemovePieceFromGame(piece);

        private void MovePieceTo(Piece piece, Tile targetTile)
        {
            Tile.TileName from = piece.TilePosition.Name;

            // Check Capture
            if (targetTile.Piece != null && piece.Team != targetTile.Piece.Team)
            {
                Capture(targetTile.Piece);
            }

            piece.MoveTo(targetTile);

            Tile.TileName to = piece.TilePosition.Name;

            // _moves.Push(new Move(_selectedPiece, from, to));
        }

        // Create traditional piece set up
        public void CreateNewDefaultGame()
        {
            RestartGame();

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

        public void CreateNewGameFromFile(string path)
        {
            RestartGame();

            // TODO
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

        private void FinishGame(Team? team)
        {
            _playerState = PlayerState.FinishedGame;
            OnGameFinished?.Invoke(team);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            HandleKeyboardInputs();

            foreach (Piece piece in _pieces)
            {
                piece.Update(gameTime);
            }
        }

        private void HandleKeyboardInputs()
        {
            if (_playerState == PlayerState.MovingPiece && MonoGameEngine.Mouse.IsRightMouseDown)
            {
                OnPieceDeselected?.Invoke();
            }

            if (Input.IsKeyCombinationDown(Keys.LeftControl, Keys.Z))
            {
                _moveManager.Undo(_tileBoard);
            }

            if (Input.IsKeyCombinationDown(Keys.LeftControl, Keys.Y))
            {
                _moveManager.Redo(_tileBoard);
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
