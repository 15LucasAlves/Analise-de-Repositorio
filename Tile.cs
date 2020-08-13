using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Chess.Pieces;

namespace Chess 
{
    // Chess board tile
    class Tile : ClickableObject
    {
        private Point coordinate;
        public Point Coordinate
        {
            get => coordinate;
            set
            {
                coordinate = value;
                TileName = new TileCoord(Coordinate);
            }
        }

        public TileCoord TileName { get; private set; }
        public Piece Piece { get; set; }

        public Tile (Rectangle rectangle, Point coordinate) : base (rectangle)
        {
            Coordinate = coordinate;
        }

        public Tile (Texture2D sprite, Rectangle rectangle, Point coordinate) : base (sprite, rectangle)
        {
            Coordinate = coordinate;
        }
    }
}
