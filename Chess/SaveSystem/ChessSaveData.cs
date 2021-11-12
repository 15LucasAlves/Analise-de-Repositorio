using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Chess
{
    [Serializable]
    class ChessSaveData
    {
        public int movesCount;
        public int[][] fromCoordinates;
        public int[][] toCoordinates;


        public ChessSaveData(IEnumerable<Move> moves)
        {
            List<Move> listOfMoves = new List<Move>(moves);

            movesCount = listOfMoves.Count;
            fromCoordinates = new int[listOfMoves.Count][];
            toCoordinates = new int[listOfMoves.Count][];

            for(int i = 0; i < movesCount; i++)
            {
                Point fromCoordinate = listOfMoves[i].from.ToCoordinate();
                Point toCoordinate = listOfMoves[i].to.ToCoordinate();

                fromCoordinates[i] = new int[2];
                fromCoordinates[i][0] = fromCoordinate.X;
                fromCoordinates[i][1] = fromCoordinate.Y;

                toCoordinates[i] = new int[2];
                toCoordinates[i][0] = toCoordinate.X;
                toCoordinates[i][1] = toCoordinate.Y;

            }
        }
    }
}
