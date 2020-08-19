using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Chess.Pieces
{
    class Bishop : Piece
    {
        public Bishop(Texture2D sprite, Team team, Tile position) : base(sprite, team, position)
        {
        }

        public override IEnumerable<Tile> GetPossibleMoves(TileBoard board)
        {
            List<Tile> possibleMoves = new List<Tile>();
            Tile tileBeingChecked;

            // Check top-right
            for (int i = TilePosition.Coordinate.X, j = TilePosition.Coordinate.Y;
                 i < board.Tiles.GetLength(0) && j < board.Tiles.GetLength(1);
                 i++, j++)
            {

                tileBeingChecked = board[i, j];

                if (IsPossibleMove(tileBeingChecked))
                    possibleMoves.Add(tileBeingChecked);

                if (tileBeingChecked.Piece != null && tileBeingChecked.Piece != this)
                    break;
            }
            // Check top-left
            for (int i = TilePosition.Coordinate.X, j = TilePosition.Coordinate.Y;
                 i >= 0 && j < board.Tiles.GetLength(1);
                 i--, j++)
            {
                tileBeingChecked = board[i, j];

                if (IsPossibleMove(tileBeingChecked))
                    possibleMoves.Add(tileBeingChecked);

                if (tileBeingChecked.Piece != null && tileBeingChecked.Piece != this)
                    break;
            }
            // Check bottom-right
            for (int i = TilePosition.Coordinate.X, j = TilePosition.Coordinate.Y;
                 i < board.Tiles.GetLength(0) && j >= 0;
                 i++, j--)
            {
                tileBeingChecked = board[i, j];

                if (IsPossibleMove(tileBeingChecked))
                    possibleMoves.Add(tileBeingChecked);

                if (tileBeingChecked.Piece != null && tileBeingChecked.Piece != this)
                    break;
            }
            // Check bottom-left
            for (int i = TilePosition.Coordinate.X, j = TilePosition.Coordinate.Y;
                 i >= 0 && j >= 0;
                 i--, j--)
            {
                tileBeingChecked = board[i, j];

                if (IsPossibleMove(tileBeingChecked))
                    possibleMoves.Add(tileBeingChecked);

                if (tileBeingChecked.Piece != null && tileBeingChecked.Piece != this)
                    break;
            }

            return possibleMoves;
        }
    }
}
