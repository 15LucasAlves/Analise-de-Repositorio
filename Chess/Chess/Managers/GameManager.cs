using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Chess.Pieces;

namespace Chess.Managers
{
    public enum PlayerState { Selecting, Moving }

    class GameManager
    {
        private readonly Color SelectedTint = Color.LightSkyBlue;
        private readonly Color PossibleMoveTint = Color.Yellow;
        private readonly Color CheckingPieceTint = Color.Red;

        private ChessBoard board;
        private Dictionary<Tuple<PieceType, Team>, Texture2D> pieceTextures;
        private ObjectContainer pieces;
        private ObjectContainer whitePieces;
        private ObjectContainer blackPieces;
        private List<Move> moves;

        // In-game logic
        private Piece selectedPiece;
        private Tile selectedPieceTile;
        private List<Tile> selectedPiecePossibleMoves;
        
        private TileBoard checkVerificationBoard;

        private King whiteKing;
        private King blackKing;

        // Properties
        private Team playerTurn;
        public Team PlayerTurn
        {
            get => playerTurn;
            private set
            {
                playerTurn = value;
                OnPlayerTurnChange?.Invoke();
            }
        }
        public PlayerState PlayerState { get; private set; }
        public int TurnCount { get; private set; }

        // Events
        public Action<Piece> OnPieceSelected;
        public Action OnPieceDeselected;
        public Action OnPlayerTurnChange;

        public Action<Team?> OnGameFinished;

        public GameManager(ChessBoard board, Dictionary<Tuple<PieceType, Team>, Texture2D> pieceTextures)
        {
            this.board = board;
            this.pieceTextures = pieceTextures;

            pieces = new ObjectContainer();
            whitePieces = new ObjectContainer();
            blackPieces = new ObjectContainer();
            whitePieces.Active(true);
            blackPieces.Active(true);
            pieces.AddChild(whitePieces, blackPieces);

            moves = new List<Move>();

            checkVerificationBoard = new TileBoard(board.BoardDimension, board.BoardDimension);

            OnPieceSelected = piece =>
            {
                selectedPiece = piece;
                selectedPieceTile = piece.TilePosition;
                selectedPiecePossibleMoves = selectedPiece.GetPossibleMoves(board.TileBoard).ToList();

                selectedPieceTile.DrawColor = SelectedTint;
                
                selectedPiecePossibleMoves.ForEach(tile => tile.DrawColor = PossibleMoveTint);
                
                PlayerState = PlayerState.Moving;
            };

            OnPieceDeselected = () =>
            {
                selectedPieceTile.DrawColor = Color.White;
                selectedPiecePossibleMoves.ForEach(tile => tile.DrawColor = Color.White);
                selectedPiecePossibleMoves = FilterMovesThatWouldPutInCheck(selectedPiece, selectedPiecePossibleMoves).ToList();
                selectedPiece = null;
                selectedPieceTile = null;
                selectedPiecePossibleMoves.Clear();

                PlayerState = PlayerState.Selecting;
            };
        }

        // Traditional piece set up
        public void CreateNewDefaultGame()
        {
            ClearBoard();

            // Black
            blackPieces.AddChild(new Bishop(pieceTextures[Tuple.Create(PieceType.Bishop, Team.Black)], Team.Black, board.GetTile(2, 0)));
            blackPieces.AddChild(new Bishop(pieceTextures[Tuple.Create(PieceType.Bishop, Team.Black)], Team.Black, board.GetTile(5, 0)));
            blackPieces.AddChild(new Knight(pieceTextures[Tuple.Create(PieceType.Knight, Team.Black)], Team.Black, board.GetTile(1, 0)));
            blackPieces.AddChild(new Knight(pieceTextures[Tuple.Create(PieceType.Knight, Team.Black)], Team.Black, board.GetTile(6, 0)));
            blackPieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.Black)], Team.Black, board.GetTile(0, 1)));
            blackPieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.Black)], Team.Black, board.GetTile(1, 1)));
            blackPieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.Black)], Team.Black, board.GetTile(2, 1)));
            blackPieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.Black)], Team.Black, board.GetTile(3, 1)));
            blackPieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.Black)], Team.Black, board.GetTile(4, 1)));
            blackPieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.Black)], Team.Black, board.GetTile(5, 1)));
            blackPieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.Black)], Team.Black, board.GetTile(6, 1)));
            blackPieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.Black)], Team.Black, board.GetTile(7, 1)));
            blackPieces.AddChild(new Queen(pieceTextures[Tuple.Create(PieceType.Queen, Team.Black)], Team.Black, board.GetTile(4, 0)));
            blackPieces.AddChild(new Rook(pieceTextures[Tuple.Create(PieceType.Rook, Team.Black)], Team.Black, board.GetTile(0, 0)));
            blackPieces.AddChild(new Rook(pieceTextures[Tuple.Create(PieceType.Rook, Team.Black)], Team.Black, board.GetTile(7, 0)));
            blackKing = new King(pieceTextures[Tuple.Create(PieceType.King, Team.Black)], Team.Black, board.GetTile(3, 0), blackKing);
            blackPieces.AddChild(blackKing);

            // White
            whitePieces.AddChild(new Bishop(pieceTextures[Tuple.Create(PieceType.Bishop, Team.White)], Team.White, board.GetTile(2, 7)));
            whitePieces.AddChild(new Bishop(pieceTextures[Tuple.Create(PieceType.Bishop, Team.White)], Team.White, board.GetTile(5, 7)));
            whitePieces.AddChild(new Knight(pieceTextures[Tuple.Create(PieceType.Knight, Team.White)], Team.White, board.GetTile(1, 7)));
            whitePieces.AddChild(new Knight(pieceTextures[Tuple.Create(PieceType.Knight, Team.White)], Team.White, board.GetTile(6, 7)));
            whitePieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.White)], Team.White, board.GetTile(0, 6)));
            whitePieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.White)], Team.White, board.GetTile(1, 6)));
            whitePieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.White)], Team.White, board.GetTile(2, 6)));
            whitePieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.White)], Team.White, board.GetTile(3, 6)));
            whitePieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.White)], Team.White, board.GetTile(4, 6)));
            whitePieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.White)], Team.White, board.GetTile(5, 6)));
            whitePieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.White)], Team.White, board.GetTile(6, 6)));
            whitePieces.AddChild(new Pawn(pieceTextures[Tuple.Create(PieceType.Pawn, Team.White)], Team.White, board.GetTile(7, 6)));
            whitePieces.AddChild(new Queen(pieceTextures[Tuple.Create(PieceType.Queen, Team.White)], Team.White, board.GetTile(4, 7)));
            whitePieces.AddChild(new Rook(pieceTextures[Tuple.Create(PieceType.Rook, Team.White)], Team.White, board.GetTile(0, 7)));
            whitePieces.AddChild(new Rook(pieceTextures[Tuple.Create(PieceType.Rook, Team.White)], Team.White, board.GetTile(7, 7)));
            whiteKing = new King(pieceTextures[Tuple.Create(PieceType.King, Team.White)], Team.White, board.GetTile(3, 7));
            whitePieces.AddChild(whiteKing);
        }

        public void CreateGameFromFile()
        {
            // TODO
        }

        public void StartGame(ObjectContainer gameScene)
        {
            SubscribeFunctionality();
            gameScene.AddChild(pieces);
            pieces.Active(true);
        }

        public void EndGame(ObjectContainer gameScene)
        {
            pieces.Active(false);
            gameScene.RemoveChild(pieces);

            ClearBoard();
        }

        #region Helper Methods
        // Invokes OnGameFinished event with the correct winner
        // if conditions have been met
        private void CheckGameFinish()
        {
            List<Tile> allPossiblePlayerMoves = new List<Tile>();

            // Black chekmated white
            if (whiteKing.InCheck(board.TileBoard))
            {
                foreach (Piece piece in whitePieces)
                    allPossiblePlayerMoves.AddRange(FilterMovesThatWouldPutInCheck(piece, piece.GetPossibleMoves(board.TileBoard)));
                    

                if (allPossiblePlayerMoves.Count <= 0)
                    OnGameFinished?.Invoke(Team.Black);
            }
            // White checkmated black
            else if (blackKing.InCheck(board.TileBoard))
            {
                foreach (Piece piece in blackPieces)
                    allPossiblePlayerMoves.AddRange(FilterMovesThatWouldPutInCheck(piece, piece.GetPossibleMoves(board.TileBoard)));

                if (allPossiblePlayerMoves.Count <= 0)
                    OnGameFinished?.Invoke(Team.White);
            }
            else
            {
                // White has no possible moves and is not in check
                foreach (Piece piece in whitePieces)
                    allPossiblePlayerMoves.AddRange(piece.GetPossibleMoves(board.TileBoard));

                if (allPossiblePlayerMoves.Count <= 0)
                    OnGameFinished?.Invoke(null);

                allPossiblePlayerMoves.Clear();

                // Black has no possible moves and is not in check
                foreach (Piece piece in blackPieces)
                    allPossiblePlayerMoves.AddRange(piece.GetPossibleMoves(board.TileBoard));

                if (allPossiblePlayerMoves.Count <= 0)
                    OnGameFinished?.Invoke(null);
            }

            // Draw if king vs king
            if (whitePieces.Count == 1 && blackPieces.Count == 1)
                OnGameFinished?.Invoke(null);

            // Draw if king and bishop vs king
            if ((whitePieces.Count == 2 && whitePieces.ToList().First(piece => piece is Bishop) != null && blackPieces.Count == 1) ||
                (blackPieces.Count == 2 && blackPieces.ToList().First(piece => piece is Bishop) != null && whitePieces.Count == 1))
                OnGameFinished?.Invoke(null);

            // Draw if king and knight vs king
            if ((whitePieces.Count == 2 && whitePieces.ToList().First(piece => piece is Knight) != null && blackPieces.Count == 1) ||
                (blackPieces.Count == 2 && blackPieces.ToList().First(piece => piece is Knight) != null && whitePieces.Count == 1))
                OnGameFinished?.Invoke(null);

            // Draw if king and bishop vs king and bishop on the same color
            if (whitePieces.Count == 2 && whitePieces.ToList().First(piece => piece is Bishop) != null && (whitePieces.ToList().First(piece => piece is Bishop) as Bishop).TilePosition.Coordinate.X % 2 == 0 &&
                blackPieces.Count == 2 && blackPieces.ToList().First(piece => piece is Bishop) != null && (blackPieces.ToList().First(piece => piece is Bishop) as Bishop).TilePosition.Coordinate.X % 2 == 0)
                OnGameFinished?.Invoke(null);

            if (whitePieces.Count == 2 && whitePieces.ToList().First(piece => piece is Bishop) != null && (whitePieces.ToList().First(piece => piece is Bishop) as Bishop).TilePosition.Coordinate.X % 2 == 1 &&
                blackPieces.Count == 2 && blackPieces.ToList().First(piece => piece is Bishop) != null && (blackPieces.ToList().First(piece => piece is Bishop) as Bishop).TilePosition.Coordinate.X % 2 == 1)
                OnGameFinished?.Invoke(null);
        }

        private void TogglePlayerTurn()
        {
            switch (PlayerTurn)
            {
                case Team.White:
                    PlayerTurn = Team.Black;
                    break;
                case Team.Black:
                    PlayerTurn = Team.White;
                    break;
            }

            TurnCount++;
            OnPlayerTurnChange?.Invoke();
        }

        private bool MovedPieceTo(Piece piece, Tile targetTile)
        {
            if (targetTile.Piece == null)
            {
                piece.MoveTo(targetTile);
                return true;
            }
            else
            {
                // Check Capture
                if (targetTile.Piece.Team != piece.Team)
                {
                    Capture(targetTile.Piece);
                    piece.MoveTo(targetTile);
                    return true;
                }
            }

            return false;
        }

        private bool MoveWouldPutInCheck(Piece piece, Tile destinationOfPiece)
        {
            checkVerificationBoard.ClearTiles();

            for (int i = 0; i < board.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < board.Tiles.GetLength(1); j++)
                {
                    if (board[j, i].Piece == piece)
                        checkVerificationBoard[destinationOfPiece.Coordinate.X, destinationOfPiece.Coordinate.Y].Piece = piece;
                    else
                        checkVerificationBoard[j, i].Piece = board[j, i].Piece;
                }
            }

            switch (piece.Team)
            {
                case Team.White:
                    if (whiteKing.InCheck(checkVerificationBoard))
                        return true;
                    break;
                case Team.Black:
                    if (blackKing.InCheck(checkVerificationBoard))
                        return true;
                    break;
            }
            return false;
        }

        private IEnumerable<Tile> FilterMovesThatWouldPutInCheck(Piece piece, IEnumerable<Tile> moves)
        {
            return moves.Where(move => !MoveWouldPutInCheck(piece, move));
        }

            private void Capture(Piece piece)
        {
            if (piece.Team == Team.White)
                whitePieces.RemoveChild(piece);
            else
                blackPieces.RemoveChild(piece);
        }

        // Removes all pieces from play
        private void ClearBoard()
        {
            whitePieces.Clear();
            blackPieces.Clear();

            PlayerTurn = Team.White;
            TurnCount = 0;

            moves.Clear();
        }

        private void SubscribeFunctionality()
        {
            void SubscribePieceFunctionalities(Piece piece)
            {
                // Selection functionality
                piece.OnClick += () =>
                {
                    switch (PlayerState)
                    {
                        case PlayerState.Selecting:
                            if (piece.Team == PlayerTurn)
                                OnPieceSelected?.Invoke(piece);
                            break;
                        case PlayerState.Moving:
                            if (piece == selectedPiece)
                                OnPieceDeselected?.Invoke();
                            else if (piece.Team == PlayerTurn)
                            {
                                OnPieceDeselected?.Invoke();
                                OnPieceSelected?.Invoke(piece);
                            }
                            break;
                    }
                };

                if (piece is Pawn)
                {
                    (piece as Pawn).OnEnPassantCapture += pieceCapture =>
                    {
                        Capture(pieceCapture);
                    };
                }
            }

            foreach (Piece piece in whitePieces)
            {
                SubscribePieceFunctionalities(piece);
            }

            foreach (Piece piece in blackPieces)
            {
                SubscribePieceFunctionalities(piece);
            }

            whiteKing.OnKingChecked += checkingPiece =>
            {
                checkingPiece.TilePosition.DrawColor = CheckingPieceTint;
            };

            blackKing.OnKingChecked += checkingPiece =>
            {
                checkingPiece.TilePosition.DrawColor = CheckingPieceTint;
            };

            board.ApplyClickFunctionalityToTiles(tile =>
            {
                if (PlayerState == PlayerState.Moving)
                {
                    if (selectedPiecePossibleMoves.Contains(tile))
                    {
                        if (MovedPieceTo(selectedPiece, tile))
                        {
                            OnPieceDeselected?.Invoke();
                            moves.Add(new Move(TurnCount, selectedPiece, tile.Coordinate));
                            CheckGameFinish();
                            board.TileBoard.ClearTilesTint();
                            TogglePlayerTurn();
                        }
                    }
                }
            });
        }
        #endregion
    }
}
