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

        public int Width { get; private set; }

        public int Height { get; private set; }


        public TileBoard(int boardWidth, int boardHeight) : base()
        {
            // Set Dimensions
            Width = boardWidth;
            Height = boardHeight;

            // Create Board
            Tiles = new Tile[Width, Height];

            // Populate it with tiles
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    // Create tile
                    Tiles[j, i] = new Tile(new Point(j, i));
                    // Add it to the children
                    Transform.AddChildren(Tiles[j, i]);
                }
            }
        }


        public int Distance(Tile tile1, Tile tile2) => (int)Vector2.Distance(tile1.Coordinate.ToVector2(), tile2.Coordinate.ToVector2());

        /// <summary>
        /// Copies all pieces in one tileboard to another.
        /// </summary>
        public void CopyTileboardInto(TileBoard tileboard)
        {
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    Tiles[x, y].Piece = tileboard.Tiles[x, y].Piece;
                }
            }
        }

        /// <summary>
        /// Returns whether a certain tile is being threatened by a piece from the opposite team.
        /// </summary>
        public bool TileIsThreatened(Tile tileBeingChecked, Team pieceTeam)
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
        public bool TileIsThreatened(Tile tileBeingChecked, Team pieceTeam, out IEnumerable<Piece> threateningPieces)
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
        /// Calls a function on all tiles of this tileboard.
        /// </summary>
        public void ApplyFunctionToAllTiles(Action<Tile> function)
        {
            foreach (Tile tile in Tiles)
            {
                function.Invoke(tile);
            }
        }

        // Common functions applied to all tiles
        public void TextureTiles(Func<Tile, Texture2D> tileTexturingFunction) => ApplyFunctionToAllTiles((tile) => tile.Texture = tileTexturingFunction(tile));
        public void ClearPiecesOnTiles() => ApplyFunctionToAllTiles((tile) => tile.Piece = null);
        public void ClearTilesTint() => ApplyFunctionToAllTiles((tile) => tile.Tint = Color.White);
    }
}
