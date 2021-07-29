using Microsoft.Xna.Framework;
using System;

namespace Chess
{
    struct TileCoord
    {
        public char Letter { get; }
        public int Number { get; }

        public string Coordinate
        {
            get
            {
                return Letter + Number.ToString();
            }
        }

        public TileCoord(char letter, int number)
        {
            Letter = letter;
            Number = number;
        }

        public TileCoord(int x, int y)
        {
            switch (x)
            {
                case 0:
                    Letter = 'A';
                    break;
                case 1:
                    Letter = 'B';
                    break;
                case 2:
                    Letter = 'C';
                    break;
                case 3:
                    Letter = 'D';
                    break;
                case 4:
                    Letter = 'E';
                    break;
                case 5:
                    Letter = 'F';
                    break;
                case 6:
                    Letter = 'G';
                    break;
                case 7:
                    Letter = 'H';
                    break;
                default:
                    throw new IndexOutOfRangeException("Horizontal coordinate outside of chess board range.");
            }

            if (y > 7 || y < 0)
                throw new IndexOutOfRangeException("Vertical coordinate outside of chess board range.");
            else
                Number = 8 - y;
        }

        public TileCoord(Point coordinate) : this(coordinate.X, coordinate.Y)
        {
        }
    }
}
