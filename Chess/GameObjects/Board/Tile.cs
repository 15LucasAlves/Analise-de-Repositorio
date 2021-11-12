using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGameEngine;

namespace Chess 
{
    // Chess board tile
    class Tile : ClickableGameObject
    {
        public struct TileName
        {
            public char Letter { get; }

            public int Number { get; }


            public TileName(char letter, int number)
            {
                Letter = letter;
                Number = number;
            }

            public TileName(int x, int y)
            {
                // Do not let coordinates go below 0
                x = MathHelper.Max(x, 0);
                y = MathHelper.Max(y, 0);

                // Convert X coordinate number to a letter
                Letter = Convert.ToChar(65 + x);
                Number = y;
            }

            public TileName(Point coordinate) : this(coordinate.X, coordinate.Y) {}


            public Point ToCoordinate() => new Point(Letter - 65, Number);

            public override string ToString() => Letter + (Number + 1).ToString();

            // String to TileName Parser
            public static TileName From(string str)
            {
                return new TileName(str[0], int.Parse(str[1].ToString()) - 1);
            }
        }


        private Point _coordinate;
        public Point Coordinate
        {
            get => _coordinate;
            set
            {
                _coordinate = value;
                Name = new TileName(Coordinate);
            }
        }

        public TileName Name { get; private set; }

        public Piece Piece { get; set; }
        public bool IsEmpty => Piece == null;


        public Tile(int x, int y) : base()
        {
            Coordinate = new Point(x, y);
        }

        public Tile (Point coordinate) : base()
        {
            Coordinate = coordinate;
        }


        protected override void OnLoad(MonoGameApp app)
        {
            base.OnLoad(app);

            // Default tile texture is the white tile
            Texture = app.Content.Load<Texture2D>("whiteTile");
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
