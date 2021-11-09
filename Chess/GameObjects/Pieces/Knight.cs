using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class Knight : Piece
    {
        public Knight() : base()
        {

        }

        protected override void OnLoad(MonoGameApp app)
        {
            base.OnLoad(app);

            if (Team == Team.White)
            {
                Texture = app.Content.Load<Texture2D>("Pieces/wh");
            }
            else
            {
                Texture = app.Content.Load<Texture2D>("Pieces/bh");
            }
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
