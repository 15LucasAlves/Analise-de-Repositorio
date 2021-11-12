using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class ChessBoard : TexturedGameObject
    {
        public const int BoardDimensions = 8;

        public TileBoard TileBoard { get; private set; }


        public ChessBoard() : base()
        {
            // Create the TileBoard
            TileBoard = new TileBoard(BoardDimensions, BoardDimensions);

            // Add it to the ChessBoard's children
            Transform.AddChildren(TileBoard);

            // Position the TileBoard correctly inside the Chessboard edge texture
            TileBoard.Transform.Position += new Vector3(38f * Transform.GlobalScale.X, 38f * Transform.GlobalScale.Y, 0);
        }
        

        protected override void OnLoad(MonoGameApp app)
        {
            base.OnLoad(app);

            Texture = app.Content.Load<Texture2D>("boardEdge");
            
            Texture2D whiteTileTexture = app.Content.Load<Texture2D>("whiteTile");
            Texture2D blackTileTexture = app.Content.Load<Texture2D>("brownTile");

            // Apply traditional chess board pattern to textures
            Func<Tile, Texture2D> tileTexturingFunction = (tile) =>
            {
                int x = tile.Coordinate.X;
                int y = tile.Coordinate.Y;

                if (x % 2 == 0 && y % 2 == 0)
                    return whiteTileTexture;
                else if (x % 2 == 1 && y % 2 == 0)
                    return blackTileTexture;
                else if (x % 2 == 0 && y % 2 == 1)
                    return blackTileTexture;
                else
                    return whiteTileTexture;
            };

            // Texture tiles according to the texturing function
            TileBoard.TextureTiles(tileTexturingFunction);

            // Position the Tile GameObjects correctly
            foreach(Tile tile in TileBoard.Tiles)
            {
                tile.Transform.Position = new Vector3(tile.Coordinate.X * tile.Texture.Bounds.Width * tile.Transform.Scale.X, (BoardDimensions - 1 - tile.Coordinate.Y) * tile.Texture.Bounds.Height * tile.Transform.Scale.Y, Transform.Position.Z);
            }
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            base.OnDraw(spriteBatch);

            foreach (Tile tile in TileBoard.Tiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}