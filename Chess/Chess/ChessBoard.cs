using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Chess
{
    class ChessBoard : GameObject
    {
        public int BoardDimension => 8;

        public TileBoard TileBoard;

        private Dictionary<TileCoord, Tile> tileDictionary;

        public Tile[,] Tiles => TileBoard.Tiles;
        public Tile this[int x, int y] => TileBoard[x, y];

        public ChessBoard(Texture2D edge, Rectangle rectangle, Texture2D brownTileTexture, Texture2D whiteTileTexture) : base(edge, rectangle)
        {
            TileBoard = new TileBoard(Rectangle.X + 41, Rectangle.Y + 38, BoardDimension, BoardDimension, 50, 50, (x, y) =>
            {
                if (x % 2 == 0 && y % 2 == 0)
                    return whiteTileTexture;
                else if (x % 2 == 1 && y % 2 == 0)
                    return brownTileTexture;
                else if (x % 2 == 0 && y % 2 == 1)
                    return brownTileTexture;
                else
                    return whiteTileTexture;
            });

            tileDictionary = new Dictionary<TileCoord, Tile>();

            foreach (Tile tile in Tiles)
                tileDictionary.Add(tile.TileName, tile);
        }

        public Tile GetTile(int x, int y)
        {
            if (x >= 0 && x < Tiles.GetLength(0) && y >= 0 && y < Tiles.GetLength(1))
                return Tiles[x, y];
            else
                return null;
        }

        public Tile GetTile(TileCoord tileCoord)
        {
            return tileDictionary[tileCoord];
        }

        public Tile FindTileOf(Chess.Pieces.Piece piece)
        {
            foreach (Tile tile in Tiles)
            {
                if (tile.Piece == piece)
                    return tile;
            }

            return null;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tile tile in Tiles)
            {
                tile.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw Edge
            base.Draw(spriteBatch);

            if (Visible)
            {
                // Draw Tiles
                foreach (Tile tile in Tiles)
                    tile.Draw(spriteBatch);
            }
        }

        public void ApplyClickFunctionalityToTiles(Action<Tile> functionality)
        {
            foreach (Tile tile in Tiles)
                tile.OnClick += () => { functionality?.Invoke(tile); };
        }
    }
}