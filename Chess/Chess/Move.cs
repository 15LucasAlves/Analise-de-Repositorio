using Microsoft.Xna.Framework;

using Chess.Pieces;

namespace Chess
{
    struct Move
    {
        public int Turn;
        public Piece Piece;
        public TileCoord TileCoord;

        public Move (int turn, Piece piece, TileCoord tileCoord)
        {
            Turn = turn;
            Piece = piece;
            TileCoord = tileCoord;
        }

        public Move (int turn, Piece piece, Point coordinate)
        {
            Turn = turn;
            Piece = piece;
            TileCoord = new TileCoord(coordinate);
        }
    }
}
