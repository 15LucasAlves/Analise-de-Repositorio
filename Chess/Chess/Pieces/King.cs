using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chess.Pieces
{
    class King : Piece
    {
        public Piece CheckingPiece { get; private set; }

        private Tile castleTrigger;
        private Piece castleRook;
        private Tile castleRookDestination;

        public Action<Piece> OnKingChecked;

        public King(Texture2D sprite, Team team, Tile position, King king) : base(sprite, team, position, king)
        {
        }

        public bool InCheck(TileBoard board)
        {
            foreach (Tile tile in board.Tiles)
            {
                if (!IsEmpty(tile))
                {
                    if (tile.Piece == this)
                        return TileIsThreatened(board, tile);
                }
            }

            return false;
        }

        public override void MoveTo(Tile tile)
        {
            base.MoveTo(tile);

            if (tile == castleTrigger)
                castleRook.MoveTo(castleRookDestination);
        }

        // Takes into account enemy threatened tiles
        protected bool IsPossibleMove(TileBoard board, Tile tile)
        {
            if (tile != null)
            {
                if (!TileIsThreatened(board, tile))
                {
                    if (IsEmpty(tile))
                        return true;
                    else
                    {
                        if (tile.Piece != this)
                        {
                            if (ContainsDifferentTeamPiece(tile))
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        public override IEnumerable<Tile> GetPossibleMoves(TileBoard board)
        {
            List<Tile> possibleMoves = new List<Tile>();
            Tile tileBeingChecked;

            // Check right
            tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y];
            if (IsPossibleMove(board, tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check left
            tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y];
            if (IsPossibleMove(board, tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check above
            tileBeingChecked = board[TilePosition.Coordinate.X, TilePosition.Coordinate.Y + 1];
            if (IsPossibleMove(board, tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check below
            tileBeingChecked = board[TilePosition.Coordinate.X, TilePosition.Coordinate.Y - 1];
            if (IsPossibleMove(board, tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check top-right
            tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y + 1];
            if (IsPossibleMove(board, tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check top-left
            tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y + 1];
            if (IsPossibleMove(board, tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check bottom-right
            tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y - 1];
            if (IsPossibleMove(board, tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check bottom-left
            tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y - 1];
            if (IsPossibleMove(board, tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);

            // Check castle
            if (Unmoved)
            {
                // King is not in check
                if (!InCheck(board))
                {
                    // Check right
                    for (int i = TilePosition.Coordinate.X; i <= board.Tiles.GetLength(0); i++)
                    {
                        tileBeingChecked = board[i, TilePosition.Coordinate.Y];

                        // If no pieces between them
                        if (!IsEmpty(tileBeingChecked))
                        {
                            // King will not pass through a square that is threatened or end up in check
                            if (TileIsThreatened(board, tileBeingChecked))
                                break;

                            if (tileBeingChecked.Piece is Rook && tileBeingChecked.Piece.Team == Team)
                            {
                                // If rook is unmoved
                                if (tileBeingChecked.Piece.Unmoved)
                                {
                                    if (IsPossibleMove(board, board[TilePosition.Coordinate.X + 2, TilePosition.Coordinate.Y]))
                                        possibleMoves.Add(board[TilePosition.Coordinate.X + 2, TilePosition.Coordinate.Y]);
                                    castleTrigger = board[TilePosition.Coordinate.X + 2, TilePosition.Coordinate.Y];
                                    castleRook = tileBeingChecked.Piece;
                                    castleRookDestination = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y];
                                }
                            }
                            // If piece that is not Rook found on path, exit
                            else
                            {
                                // Make sure it is not this piece
                                if (tileBeingChecked.Piece != this)
                                    break;
                            }
                        }
                    }

                    // Check left
                    for (int i = TilePosition.Coordinate.X; i >= 0; i--)
                    {
                        tileBeingChecked = board[i, TilePosition.Coordinate.Y];

                        // If no pieces between them
                        if (!IsEmpty(tileBeingChecked))
                        {
                            // King will not pass through a square that is threatened or end up in check
                            if (TileIsThreatened(board, tileBeingChecked))
                                break;

                            if (tileBeingChecked.Piece is Rook && tileBeingChecked.Piece.Team == Team)
                            {
                                // If rook is unmoved
                                if (tileBeingChecked.Piece.Unmoved)
                                {
                                    if (IsPossibleMove(board, board[TilePosition.Coordinate.X - 2, TilePosition.Coordinate.Y]))
                                        possibleMoves.Add(board[TilePosition.Coordinate.X - 2, TilePosition.Coordinate.Y]);
                                    castleTrigger = board[TilePosition.Coordinate.X - 2, TilePosition.Coordinate.Y];
                                    castleRook = tileBeingChecked.Piece;
                                    castleRookDestination = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y];
                                }
                            }
                            // If piece that is not Rook found on path, exit
                            else
                            {
                                // Make sure it is not this piece
                                if (tileBeingChecked.Piece != this)
                                    break;
                            }
                        }
                    }
                }
            }

            return possibleMoves;
        }

        #region Helper Methods
        // Returns whether a certain tile is being threatened by a piece
        private bool TileIsThreatened(TileBoard board, Tile tile)
        {
            foreach (Tile boardTile in board.Tiles)
            {
                if (boardTile.Piece != null)
                {
                    if (boardTile.Piece.Team != Team)
                    {
                        if (boardTile.Piece is King)
                        {
                            // To avoid infinite GetPossibleMoves(Tileboard) calls
                            // between the two kings
                            if (board.Distance(tile, boardTile) <= 1)
                                return true;
                        }
                        else
                        {
                            if (boardTile.Piece.GetPossibleMoves(board).Contains(tile))
                            {
                                if (tile == TilePosition)
                                {
                                    CheckingPiece = boardTile.Piece;
                                    OnKingChecked?.Invoke(boardTile.Piece);
                                }

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        #endregion
    }
}
