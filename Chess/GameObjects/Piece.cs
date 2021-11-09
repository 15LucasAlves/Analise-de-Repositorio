using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameEngine;

namespace Chess
{
    public enum Team { White, Black }

    abstract class Piece : ClickableGameObject, ILoadable
    {
        public Team Team { get; set; }

        private Tile _tilePosition;
        /// <summary>
        /// Hard position set. Use MoveTo for correct in-game functionality
        /// </summary>
        public Tile TilePosition
        {
            get => _tilePosition;
            set
            {
                if (_tilePosition != null)
                {
                    _tilePosition.Piece = null;
                }

                _tilePosition = value;

                _tilePosition.Piece = this;

                ClickableArea = _tilePosition.ClickableArea;
            }
        }

        public bool Unmoved { get; set; } = true;

        // Method to be overriden by all pieces to determine the tiles it can move to
        public abstract IEnumerable<Tile> GetPossibleMoves(TileBoard board);

        public virtual void MoveTo(Tile tile)
        {
            Unmoved = false;

            if (_tilePosition != null)
            {
                _tilePosition.Piece = null;
            }

            _tilePosition = tile;

            _tilePosition.Piece = this;

            ClickableArea = _tilePosition.ClickableArea;
        }

        public virtual bool IsPossibleMove(Tile tile)
        {
            if (tile != null)
            {
                // Avoid adding this piece's current tile to the possible moves
                if (tile != TilePosition)
                {
                    // If tile is empty
                    // Or there is a piece not in the same team as this one
                    if (tile.IsEmpty || IsDifferentTeam(tile.Piece))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsDifferentTeam(Piece piece)
        {
            return piece.Team != Team;
        }

        public override string ToString()
        {
            return Team.ToString() + " " + GetType().Name;
        }

        // String to Piece Parser
        public static Piece From(string str)
        {
            string[] data = str.Split(' ');

            Team team = (Team)Enum.Parse(typeof(Team), data[0]);
            Type pieceType = Type.GetType(data[1]);

            Piece piece = Activator.CreateInstance(pieceType) as Piece;
            piece.Team = team;

            return piece;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            float lerpSpeed = 16f;
            Transform.Position = Vector3.Lerp(Transform.Position, new Vector3(TilePosition.Transform.GlobalPosition.X, TilePosition.Transform.GlobalPosition.Y, Transform.Position.Z), (float)gameTime.ElapsedGameTime.TotalSeconds * lerpSpeed);
        }
    }
}
