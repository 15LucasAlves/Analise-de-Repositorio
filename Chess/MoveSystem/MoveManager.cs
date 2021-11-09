using Microsoft.Xna.Framework;

namespace Chess
{
    class MoveManager
    {
        private BackAndForwardStack<Move> _moveStack = new BackAndForwardStack<Move>();

        public MoveManager()
        {
        }

        public void Execute(Move move, TileBoard board)
        {
            Point fromCoordinate = move.from.ToCoordinate();
            Point toCoordinate = move.to.ToCoordinate();

            Piece piece = board[fromCoordinate.X, fromCoordinate.Y].Piece;
            piece.MoveTo(board[toCoordinate.X, toCoordinate.Y]);
        }

        public void ExecuteInverse(Move move, TileBoard board)
        {
            Point fromCoordinate = move.from.ToCoordinate();
            Point toCoordinate = move.to.ToCoordinate();

            Piece piece = board[toCoordinate.X, toCoordinate.Y].Piece;
            piece.MoveTo(board[fromCoordinate.X, fromCoordinate.Y]);
        }

        public void Undo(TileBoard board)
        {
            ExecuteInverse(_moveStack.Back(), board);
        }

        public void Redo(TileBoard board)
        {
            Execute(_moveStack.Back(), board);
        }
    }
}