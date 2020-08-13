using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Chess.Pieces
{
    class Knight : Piece
    {
        public Knight(Texture2D sprite, Team team, Tile position) : base(sprite, team, position)
        {
        }

        public override IEnumerable<Tile> GetPossibleMoves(TileBoard board)
        {
            List<Tile> possibleMoves = new List<Tile>();
            Tile tileBeingChecked;

            // Check top-right
            tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y + 2];
            if (IsPossibleMove(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check top-left
            tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y + 2];
            if (IsPossibleMove(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check bottom-right
            tileBeingChecked = board[TilePosition.Coordinate.X + 1, TilePosition.Coordinate.Y - 2];
            if (IsPossibleMove(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check bottom-left
            tileBeingChecked = board[TilePosition.Coordinate.X - 1, TilePosition.Coordinate.Y - 2];
            if (IsPossibleMove(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check right-top
            tileBeingChecked = board[TilePosition.Coordinate.X + 2, TilePosition.Coordinate.Y + 1];
            if (IsPossibleMove(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check right-bottom
            tileBeingChecked = board[TilePosition.Coordinate.X + 2, TilePosition.Coordinate.Y - 1];
            if (IsPossibleMove(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check left-top
            tileBeingChecked = board[TilePosition.Coordinate.X - 2, TilePosition.Coordinate.Y + 1];
            if (IsPossibleMove(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);
            // Check left-bottom
            tileBeingChecked = board[TilePosition.Coordinate.X - 2, TilePosition.Coordinate.Y - 1];
            if (IsPossibleMove(tileBeingChecked))
                possibleMoves.Add(tileBeingChecked);

            return possibleMoves;
        }
    }
}
