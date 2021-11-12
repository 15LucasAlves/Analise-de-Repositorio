using Microsoft.Xna.Framework;

namespace Chess
{
    class MoveCommand : ICommand
    {
        public Move Move { get; private set; }

        private ChessGameManager _gameManager;
        private TileBoard _board;

        public MoveCommand(Move move, ChessGameManager gameManager, TileBoard board)
        {
            Move = move;
            _gameManager = gameManager;
            _board = board;
        }

        public void Execute()
        {
            Point fromCoordinate = Move.from.ToCoordinate();
            Point toCoordinate = Move.to.ToCoordinate();

            Piece piece = _board[fromCoordinate.X, fromCoordinate.Y].Piece;
            Tile tile = _board[toCoordinate.X, toCoordinate.Y];

            _gameManager.MovePieceTo(piece, tile);

            _gameManager.DeselectSelectedPiece();

            _board.ClearTilesTint();

            _gameManager.TurnManager.NextTurn();

            _gameManager.CheckGameState();
        }

        public void Undo()
        {
            if (_gameManager.StateMachine.CurrentState is WaitingOnFinishedGame)
            {
                _gameManager.ReturnFromGameFinished();
            }

            _gameManager.StateMachine.SetState<SelectingPiece>();

            Point toCoordinate = Move.to.ToCoordinate();

            Piece piece = _board[toCoordinate.X, toCoordinate.Y].Piece;

            _gameManager.MovePieceBack(piece);

            _gameManager.DeselectSelectedPiece();

            _board.ClearTilesTint();

            _gameManager.TurnManager.PreviousTurn();

            _gameManager.CheckGameState();
        }
    }
}
