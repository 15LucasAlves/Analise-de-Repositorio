using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Chess
{
    class TileBoard
    {
        public Tile[,] Tiles;

        public Tile this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < Tiles.GetLength(0) && y >= 0 && y < Tiles.GetLength(1))
                    return Tiles[x, y];
                else
                    return null;
            }
        }

        public TileBoard(int width, int height)
        {
            // Create Board
            Tiles = new Tile[width, height];

            // Populate it with tiles
            Rectangle tilePosition = new Rectangle(0, 0, 0, 0);

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[j, i] = new Tile(new Rectangle(0, 0, 0, 0), new Point(j, i));
                    tilePosition = new Rectangle(tilePosition.X, tilePosition.Y, 0, 0);
                }

                tilePosition = new Rectangle(0, (1 + i), 0, 0);
            }
        }

        public TileBoard(int width, int height, int tileWidth, int tileHeight, Texture2D tileTexture)
        {
            // Create Board
            Tiles = new Tile[width, height];

            Rectangle tilePosition = new Rectangle(0, 0, tileWidth, tileHeight);

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[j, i] = new Tile(tileTexture, new Rectangle(0, 0, tileWidth, tileHeight), new Point(j, i));
                    tilePosition = new Rectangle(tilePosition.X + tileWidth, tilePosition.Y, tileWidth, tileHeight);
                }

                tilePosition = new Rectangle(0, (1 + i) * tileHeight, tileWidth, tileHeight);
            }
        }

        public TileBoard(int width, int height, int tileWidth, int tileHeight, Func<Texture2D> tileTexturingPattern)
        {
            // Create Board
            Tiles = new Tile[width, height];

            Rectangle tilePosition = new Rectangle(0, 0, tileWidth, tileHeight);

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[j, i] = new Tile(tileTexturingPattern?.Invoke(), new Rectangle(0, 0, tileWidth, tileHeight), new Point(j, i));
                    tilePosition = new Rectangle(tilePosition.X + tileWidth, tilePosition.Y, tileWidth, tileHeight);
                }
                tilePosition = new Rectangle(0, (1 + i) * tileHeight, tileWidth, tileHeight);
            }
        }

        public TileBoard(int x, int y, int width, int height, int tileWidth, int tileHeight, Func<int, int, Texture2D> tileTexturingPattern)
        {
            // Create Board
            Tiles = new Tile[width, height];

            // Populate it with tiles
            Rectangle tilePosition = new Rectangle(x, y, tileWidth, tileHeight);

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[j, i] = new Tile(tileTexturingPattern?.Invoke(j, i), tilePosition, new Point(j, i));
                    tilePosition = new Rectangle(tilePosition.X + tileWidth, tilePosition.Y, tileWidth, tileHeight);
                }
                tilePosition = new Rectangle(x, y + (1 + i) * tileHeight, tileWidth, tileHeight);
            }
        }

        public void Translate(int x, int y)
        {
            foreach (Tile tile in Tiles)
                tile.Rectangle.Offset(x, y);
        }

        // Taxicab distance between two tiles
        public int Distance(Tile tile1, Tile tile2)
        {
            return (int)Vector2.Distance(tile1.Coordinate.ToVector2(), tile2.Coordinate.ToVector2());
        }

        public void ClearTiles()
        {
            foreach (Tile tile in Tiles)
                tile.Piece = null;
        }

        public void ClearTilesTint()
        {
            foreach (Tile tile in Tiles)
                tile.DrawColor = Color.White;
        }
    }
}
