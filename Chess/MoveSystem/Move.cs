namespace Chess
{
    struct Move
    {
        public Piece piece;
        public Tile.TileName from;
        public Tile.TileName to;

        public Move(Piece piece, Tile.TileName from, Tile.TileName to)
        {
            this.piece = piece;
            this.from = from;
            this.to = to;
        }

        // Traditional string of how a move is written down (e.g. White Pawn C3)
        public override string ToString()
        {
            return piece.ToString() + " " + to.ToString();
        }

        // String a Move object can be reconstructed from
        public string ToEncodedString()
        {
            return piece.ToString() + " " + from.ToString() + to.ToString();
        }

        // String to Move Parser
        public static Move From(string str)
        {
            string[] data = str.Split(' ');
            Piece piece = Piece.From(data[0] + data[1]);
            Tile.TileName from = Tile.TileName.From(data[2]);
            Tile.TileName to = Tile.TileName.From(data[3]);

            return new Move(piece, from, to);
        }
    }
}
