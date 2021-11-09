using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class Pawn : Piece
    {
        private bool _enPassantFlag;
        private Tile _enPassantFlagTrigger;

        private Tile _enPassantCaptureTrigger;
        private Pawn _enPassantCapturePawn;

        public Action<Pawn> OnEnPassantCapture;

        public Pawn() : base()
        {

        }

        protected override void OnLoad(MonoGameApp app)
        {
            base.OnLoad(app);

            if (Team == Team.White)
            {
                Texture = app.Content.Load<Texture2D>("Pieces/wp");
            }
            else
            {
                Texture = app.Content.Load<Texture2D>("Pieces/bp");
            }
        }

        public override void MoveTo(Tile tile)
        {
            if (tile == _enPassantFlagTrigger)
                _enPassantFlag = true;
            else
                _enPassantFlag = false;

            if (tile == _enPassantCaptureTrigger)
                OnEnPassantCapture?.Invoke(_enPassantCapturePawn);

            base.MoveTo(tile);
        }

        public override IEnumerable<Tile> GetPossibleMoves(TileBoard board)
        {
            List<Tile> possibleMoves = new List<Tile>();
            Tile tileBeingChecked;

            int yDirection = GetYDirection();

            // Check front
            tileBeingChecked = board[TilePosition.Coordinate.X, TilePosition.Coordinate.Y + yDirection];
            if (IsPossibleMove(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);

            // Initial 2 tile move possibility
            if (Unmoved)
            {
                // If moving to the first tile was possible
                if (IsPossibleMove(tileBeingChecked))
                {
                    // Check the one after
                    tileBeingChecked = board[TilePosition.Coordinate.X, TilePosition.Coordinate.Y + yDirection * 2];

                    if (IsPossibleMove(tileBeingChecked))
                    {
                        possibleMoves.Add(tileBeingChecked);
                        _enPassantFlagTrigger = tileBeingChecked;
                    }
                }
            }

            // Capturing
            // Left
            tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y + yDirection];
            if (IsPossibleCapture(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Right
            tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y + yDirection];
            if (IsPossibleCapture(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);

            // En passant
            // Left
            tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y];
            if (tileBeingChecked != null)
            {
                if (!tileBeingChecked.IsEmpty)
                {
                    if (IsDifferentTeam(tileBeingChecked.Piece))
                    {
                        if (tileBeingChecked.Piece is Pawn)
                        {
                            if ((tileBeingChecked.Piece as Pawn)._enPassantFlag)
                            {
                                _enPassantCapturePawn = tileBeingChecked.Piece as Pawn;

                                tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y + yDirection];
                                if (IsPossibleMove(tileBeingChecked))
                                {
                                    possibleMoves.Add(tileBeingChecked);
                                    _enPassantCaptureTrigger = tileBeingChecked;    
                                }
                            }
                        }
                    }
                }
            }

            // Right
            tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y];
            if (tileBeingChecked != null)
            {
                if (tileBeingChecked.Piece != null)
                {
                    if (!tileBeingChecked.IsEmpty)
                    {
                        if (IsDifferentTeam(tileBeingChecked.Piece))
                        {
                            if (tileBeingChecked.Piece is Pawn)
                            {
                                if ((tileBeingChecked.Piece as Pawn)._enPassantFlag)
                                {
                                    _enPassantCapturePawn = tileBeingChecked.Piece as Pawn;

                                    tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y + yDirection];
                                    if (IsPossibleMove(tileBeingChecked))
                                    {
                                        possibleMoves.Add(tileBeingChecked);
                                        _enPassantCaptureTrigger = tileBeingChecked;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return possibleMoves;
        }

        public override bool IsPossibleMove(Tile tile)
        {
            if (tile != null)
            {
                // Avoid adding this piece's current tile to the possible moves
                if (tile != TilePosition)
                {
                    // If tile is empty
                    if (tile.IsEmpty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsPossibleCapture(Tile tile)
        {
            if (tile != null)
            {
                if (!tile.IsEmpty && IsDifferentTeam(tile.Piece))
                {
                    return true;
                }
            }

            return false;
        }

        public int GetYDirection()
        {
            switch (Team)
            {
                case Team.White:
                    return 1;
                case Team.Black:
                    return -1;
                default:
                    return 0;
            }
        }
    }
}
