using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Chess.Pieces
{
    class Pawn : Piece
    {
        private bool enPassantFlag;
        private Tile flagTrigger;

        public Action<Piece> OnEnPassantCapture;
        private Tile captureTrigger;
        private Piece enPassantCapture;
        private Texture2D texture2D;
        private Team black;
        private Tile tile;

        public Pawn(Texture2D sprite, Team team, Tile position, King king) : base(sprite, team, position, king)
        {
            enPassantFlag = false;
            flagTrigger = null;
        }

        public Pawn(Texture2D texture2D, Team black, Tile tile)
        {
            this.texture2D = texture2D;
            this.black = black;
            this.tile = tile;
        }

        public override void MoveTo(Tile tile)
        {
            base.MoveTo(tile);

            if (tile == flagTrigger)
                enPassantFlag = true;
            else
                enPassantFlag = false;

            if (tile == captureTrigger)
                OnEnPassantCapture?.Invoke(enPassantCapture);
        }

        public override IEnumerable<Tile> GetPossibleMoves(TileBoard board)
        {
            List<Tile> possibleMoves = new List<Tile>();
            Tile tileBeingChecked;

            switch (Team)
            {
                case Team.White:
                    // Check front
                    tileBeingChecked = board[TilePosition.Coordinate.X, TilePosition.Coordinate.Y - 1];
                    if (IsPossibleMove(board, tileBeingChecked))
                        possibleMoves.Add(tileBeingChecked);

                    // Initial 2 tile move
                    if (Unmoved)
                    {
                        if (IsPossibleMove(board, tileBeingChecked))
                        {
                            tileBeingChecked = board[TilePosition.Coordinate.X, TilePosition.Coordinate.Y - 2];

                            if (IsPossibleMove(board, tileBeingChecked))
                            {
                                possibleMoves.Add(tileBeingChecked);
                                flagTrigger = tileBeingChecked;
                            }
                        }
                    }

                    // Capturing
                    // Left
                    tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y - 1];
                    if (IsPossiblePawnCapture(tileBeingChecked))
                        possibleMoves.Add(tileBeingChecked);
                    // Right
                    tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y - 1];
                    if (IsPossiblePawnCapture(tileBeingChecked))
                        possibleMoves.Add(tileBeingChecked);
                    break;
                case Team.Black:
                    // Check front
                    tileBeingChecked = board[TilePosition.Coordinate.X, TilePosition.Coordinate.Y + 1];
                    if (IsPossibleMove(board, tileBeingChecked))
                        possibleMoves.Add(tileBeingChecked);

                    // Initial 2 tile move
                    if (Unmoved)
                    {
                        if (IsPossibleMove(board, tileBeingChecked))
                        {
                            tileBeingChecked = board[TilePosition.Coordinate.X, TilePosition.Coordinate.Y + 2];

                            if (IsPossibleMove(board, tileBeingChecked))
                            {
                                possibleMoves.Add(tileBeingChecked);
                                flagTrigger = tileBeingChecked;
                            }
                        }
                    }

                    // Capturing
                    // Left
                    tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y + 1];
                    if (IsPossiblePawnCapture(tileBeingChecked))
                        possibleMoves.Add(tileBeingChecked);
                    // Right
                    tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y + 1];
                    if (IsPossiblePawnCapture(tileBeingChecked))
                        possibleMoves.Add(tileBeingChecked);
                    break;
            }

            // En passant
            // Left
            tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y];
            if (tileBeingChecked != null)
            {
                if (tileBeingChecked.Piece is Pawn)
                {
                    if ((tileBeingChecked.Piece as Pawn).enPassantFlag)
                    {
                        Tile passantMoveTile;

                        switch (Team)
                        {
                            case Team.White:
                                passantMoveTile = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y - 1];
                                if (IsPossibleMove(board, passantMoveTile) || IsPossiblePawnCapture(passantMoveTile))
                                {
                                    possibleMoves.Add(passantMoveTile);
                                    captureTrigger = passantMoveTile;
                                }
                                break;
                            case Team.Black:
                                passantMoveTile = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y + 1];
                                if (IsPossibleMove(board, passantMoveTile) || IsPossiblePawnCapture(passantMoveTile))
                                {
                                    possibleMoves.Add(passantMoveTile);
                                    captureTrigger = passantMoveTile;
                                }
                                break;
                        }
                        
                        enPassantCapture = tileBeingChecked.Piece;
                    }
                }
            }

            // Right
            tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y];
            if (tileBeingChecked != null)
            {
                if (tileBeingChecked.Piece is Pawn)
                {
                    if ((tileBeingChecked.Piece as Pawn).enPassantFlag)
                    {
                        Tile passantMoveTile;

                        switch (Team)
                        {
                            case Team.White:
                                passantMoveTile = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y - 1];
                                if (IsPossibleMove(board, passantMoveTile) || IsPossiblePawnCapture(passantMoveTile))
                                {
                                    possibleMoves.Add(passantMoveTile);
                                    captureTrigger = passantMoveTile;
                                }
                                break;
                            case Team.Black:
                                passantMoveTile = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y + 1];
                                if (IsPossibleMove(board, passantMoveTile) || IsPossiblePawnCapture(passantMoveTile))
                                {
                                    possibleMoves.Add(passantMoveTile);
                                    captureTrigger = passantMoveTile;
                                }
                                break;
                        }

                        enPassantCapture = tileBeingChecked.Piece;
                    }
                }
            }

            return possibleMoves;
        }

        #region Helper Methods
        private bool IsPossiblePawnCapture(Tile tile)
        {
            if (tile != null)
            {
                if (!IsEmpty(tile))
                {
                    if (tile.Piece != this)
                    {
                        if (ContainsDifferentTeamPiece(tile))
                        {
                            if (!(tile.Piece is King))
                                return true;
                        }
                    }
                }
            }

            return false;
        }
        #endregion
    }
}
