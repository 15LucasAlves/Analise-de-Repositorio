using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public enum Team { White, Black }
public enum PieceType { King, Queen, Rook, Knight, Bishop, Pawn }

namespace Chess.Pieces
{
    abstract class Piece : ClickableSprite
    {
        public Tile TilePosition { get; set; }
        public Team Team { get; private set; }
        public bool Unmoved { get; private set; }
        public King King { get; private set; }

        public Piece (Texture2D sprite, Team team, Tile position, King king) : base(sprite, position.Rectangle)
        {
            this.TilePosition = position;
            this.TilePosition.Piece = this;
            this.Team = team;
            this.King = king;
            Unmoved = true;
        }

        public virtual void MoveTo(Tile tile)
        {
            Unmoved = false;

            TilePosition.Piece = null;
            TilePosition = tile;
            TilePosition.Piece = this;

            Rectangle = tile.Rectangle;
        }

        public abstract IEnumerable<Tile> GetPossibleMoves(TileBoard board);

        public override string ToString()
        {
            return Team.ToString() + " " + this.GetType().Name;
        }

        #region Helper Methods
        protected virtual bool IsPossibleMove(TileBoard board, Tile tile)
        {
            if (tile != null)
            {
                if (IsEmpty(tile))
                    return true;
                else
                {
                    if (tile.Piece != this)
                    {
                        if (ContainsDifferentTeamPiece(tile))
                        {
                            if (!King.InCheck(board))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected bool ContainsDifferentTeamPiece(Tile tile)
        {
            if (tile.Piece != null)
                return tile.Piece.Team != Team;
            else
                return false;
        }

        protected bool IsEmpty(Tile tile)
        {
            if (tile != null)
                return tile.Piece == null;

            return false;
        }
        #endregion
    }
}
