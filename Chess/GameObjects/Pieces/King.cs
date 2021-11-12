using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class King : Piece
    {
        private Tile _castleTrigger;
        private Rook _castleRook;
        private Tile _castleRookDestination;

        public event Action<Rook, Tile> OnCastle;

        public King() : base()
        {

        }

        protected override void OnLoad(MonoGameApp app)
        {
            base.OnLoad(app);

            if (Team == Team.White)
            {
                Texture = app.Content.Load<Texture2D>("Pieces/wk");
            }
            else
            {
                Texture = app.Content.Load<Texture2D>("Pieces/bk");
            }
        }

        public bool InCheck(TileBoard board)
        {
            return board.IsTileThreatened(TilePosition, Team);
        }

        public bool InCheck(TileBoard board, out IEnumerable<Piece> checkingPieces)
        {
            return board.IsTileThreatened(TilePosition, Team, out checkingPieces);
        }

        public override void MoveTo(Tile tile)
        {
            if (tile == _castleTrigger)
                OnCastle?.Invoke(_castleRook, _castleRookDestination);

            base.MoveTo(tile);
        }

        // Takes into account enemy threatened tiles
        protected bool IsPossibleMove(TileBoard board, Tile tile)
        {
            return IsPossibleMove(tile) && !board.IsTileThreatened(tile, Team);
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
            // If King has not moved
            if (Unmoved)
            {
                // If King is not in check
                if (!InCheck(board))
                {
                    // Check right
                    for (int i = TilePosition.Coordinate.X + 1; i < board.Tiles.GetLength(0); i++)
                    {
                        tileBeingChecked = board[i, TilePosition.Coordinate.Y];

                        // If tile is not null
                        if (tileBeingChecked != null)
                        {
                            // King would pass through a square that is threatened
                            if (board.IsTileThreatened(tileBeingChecked, Team))
                            {
                                break;
                            }

                            // If a piece is found
                            if (!tileBeingChecked.IsEmpty)
                            {
                                // If the piece is a Rook from the same team
                                if (tileBeingChecked.Piece is Rook && !IsDifferentTeam(tileBeingChecked.Piece))
                                {
                                    // If that Rook is unmoved
                                    if (tileBeingChecked.Piece.Unmoved)
                                    {
                                        // Add this castle as a possible move
                                        if (IsPossibleMove(board, board[TilePosition.Coordinate.X + 2, TilePosition.Coordinate.Y]))
                                            possibleMoves.Add(board[TilePosition.Coordinate.X + 2, TilePosition.Coordinate.Y]);

                                        // Set the castle trigger tile
                                        _castleTrigger = board[TilePosition.Coordinate.X + 2, TilePosition.Coordinate.Y];
                                        // Set the Rook to castle with
                                        _castleRook = tileBeingChecked.Piece as Rook;
                                        // Set the Tile the Rook will be moved to
                                        _castleRookDestination = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y];
                                    }
                                }
                                // If the piece was not a Rook from the same time
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }

                    // Check left
                    for (int i = TilePosition.Coordinate.X - 1; i >= 0; i--)
                    {
                        tileBeingChecked = board[i, TilePosition.Coordinate.Y];

                        // If tile is not null
                        if (tileBeingChecked != null)
                        {
                            // King would pass through a square that is threatened
                            if (board.IsTileThreatened(tileBeingChecked, Team))
                            {
                                break;
                            }

                            // If a piece is found
                            if (!tileBeingChecked.IsEmpty)
                            {
                                // If the piece is a Rook from the same team
                                if (tileBeingChecked.Piece is Rook && !IsDifferentTeam(tileBeingChecked.Piece))
                                {
                                    // If that Rook is unmoved
                                    if (tileBeingChecked.Piece.Unmoved)
                                    {
                                        // Add this castle as a possible move
                                        if (IsPossibleMove(board, board[TilePosition.Coordinate.X - 2, TilePosition.Coordinate.Y]))
                                            possibleMoves.Add(board[TilePosition.Coordinate.X - 2, TilePosition.Coordinate.Y]);

                                        // Set the castle trigger tile
                                        _castleTrigger = board[TilePosition.Coordinate.X - 2, TilePosition.Coordinate.Y];
                                        // Set the Rook to castle with
                                        _castleRook = tileBeingChecked.Piece as Rook;
                                        // Set the Tile the Rook will be moved to
                                        _castleRookDestination = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y];
                                    }
                                }
                                // If the piece was not a Rook from the same time
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return possibleMoves;
        }
    }
}