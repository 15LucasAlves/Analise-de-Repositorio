using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess
{
    class TileBoard : GameObject
    {
        public Tile[,] Tiles { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }


        public TileBoard(int width, int height) : base()
        {
            // Create tiles array
            Tiles = new Tile[width, height];

            // Set Dimensions
            Width = width;
            Height = height;

            // Populate tiles array with new tiles
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // Create tile
                    Tiles[j, i] = new Tile(j, i);

                    // Add tile to children
                    Transform.AddChildren(Tiles[j, i]);
                }
            }
        }


        // Tiles array accessors
        public Tile this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < Tiles.GetLength(0) && y >= 0 && y < Tiles.GetLength(1))
                {
                    return Tiles[x, y];
                }

                return null;
            }
        }

        public Tile GetTile(int x, int y) => Tiles[x, y];

        public Tile GetTile(Point coordinate) => Tiles[coordinate.X, coordinate.Y];

        public Tile GetTile(Tile.TileName tileName)
        {
            Point coordinate = tileName.ToCoordinate();
            return Tiles[coordinate.X, coordinate.Y];
        }

        public Tile GetTile(string name)
        {
            Tile.TileName tileName = Tile.TileName.From(name);
            Point coordinate = tileName.ToCoordinate();
            return Tiles[coordinate.X, coordinate.Y];
        }


        public int Distance(Tile tile1, Tile tile2) => (int)Vector2.Distance(tile1.Coordinate.ToVector2(), tile2.Coordinate.ToVector2());


        /// <summary>
        /// Returns whether a certain tile is being threatened by a piece from the opposite team.
        /// </summary>
        public bool IsTileThreatened(Tile tileBeingChecked, Team pieceTeam)
        {
            foreach (Tile tile in Tiles)
            {
                if (tile.Piece != null)
                {
                    if (tile.Piece.Team != pieceTeam)
                    {
                        // Avoid infinite GetPossibleMoves() calls between Kings
                        if (tile.Piece is King)
                        {
                            if (Distance(tile, tileBeingChecked) <= 1)
                            {
                                return true;
                            }
                        }
                        // Handle Pawn's special capture mechanics
                        else if (tile.Piece is Pawn)
                        {
                            int yDirection = (tile.Piece as Pawn).GetYDirection();

                            Tile leftCaptureTile = this[tile.Piece.TilePosition.Coordinate.X - 1, tile.Piece.TilePosition.Coordinate.Y + yDirection];
                            Tile rightCaptureTile = this[tile.Piece.TilePosition.Coordinate.X + 1, tile.Piece.TilePosition.Coordinate.Y + yDirection];

                            if (tileBeingChecked == leftCaptureTile && tile.Piece.IsPossibleMove(leftCaptureTile))
                            {
                                return true;
                            }

                            if (tileBeingChecked == rightCaptureTile && tile.Piece.IsPossibleMove(rightCaptureTile))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (tile.Piece.GetPossibleMoves(this).ToList().Contains(tileBeingChecked))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns whether a certain tile is being threatened by a piece from the opposite team.
        /// Returns which pieces are threatening specifically as an out parameter.
        /// </summary>
        public bool IsTileThreatened(Tile tileBeingChecked, Team pieceTeam, out IEnumerable<Piece> threateningPieces)
        {
            List<Piece> listOfThreateningPieces = new List<Piece>();

            foreach (Tile tile in Tiles)
            {
                if (tile.Piece != null)
                {
                    if (tile.Piece.Team != pieceTeam)
                    {
                        // Avoid infinite GetPossibleMoves() calls between Kings
                        if (tile.Piece is King)
                        {
                            if (Distance(tile, tileBeingChecked) <= 1)
                            {
                                listOfThreateningPieces.Add(tile.Piece);
                            }
                        }
                        // Handle Pawn's special capture mechanics
                        else if (tile.Piece is Pawn)
                        {
                            int yDirection = (tile.Piece as Pawn).GetYDirection();

                            Tile leftCaptureTile = this[tile.Piece.TilePosition.Coordinate.X - 1, tile.Piece.TilePosition.Coordinate.Y + yDirection];
                            Tile rightCaptureTile = this[tile.Piece.TilePosition.Coordinate.X + 1, tile.Piece.TilePosition.Coordinate.Y + yDirection];

                            if (tileBeingChecked == leftCaptureTile && tile.Piece.IsPossibleMove(leftCaptureTile))
                            {
                                listOfThreateningPieces.Add(tile.Piece);
                            }

                            if (tileBeingChecked == rightCaptureTile && tile.Piece.IsPossibleMove(rightCaptureTile))
                            {
                                listOfThreateningPieces.Add(tile.Piece);
                            }
                        }
                        else
                        {
                            if (tile.Piece.GetPossibleMoves(this).ToList().Contains(tileBeingChecked))
                            {
                                listOfThreateningPieces.Add(tile.Piece);
                            }
                        }
                    }
                }
            }

            threateningPieces = listOfThreateningPieces;
            return listOfThreateningPieces.Count > 0;
        }


        /// <summary>
        /// Calls a provided function on all tiles on this TileBoard.
        /// </summary>
        public void CallFunctionOnAllTiles(Action<Tile> function)
        {
            foreach (Tile tile in Tiles)
            {
                function.Invoke(tile);
            }
        }

        // Common functions applied to all tiles
        public void TextureTiles(Func<Tile, Texture2D> tileTexturingFunction) => CallFunctionOnAllTiles((tile) => tile.Texture = tileTexturingFunction(tile));
        public void ClearPiecesOnTiles() => CallFunctionOnAllTiles((tile) => tile.Piece = null);
        public void ClearTilesTint() => CallFunctionOnAllTiles((tile) => tile.Tint = Color.White);
    }
}
