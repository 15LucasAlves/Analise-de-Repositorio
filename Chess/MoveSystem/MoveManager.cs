using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    class MoveManager
    {
        private UndoRedoStack<MoveCommand> _moveStack = new UndoRedoStack<MoveCommand>();

        private ChessGameManager _gameManager;
        private TileBoard _board;

        public MoveManager(ChessGameManager gameManager, TileBoard board)
        {
            _gameManager = gameManager;
            _board = board;
        }

        public IEnumerable<Move> GetMovesSnapshot()
        {
            return _moveStack.BackCollection().Select(MoveCommand => MoveCommand.Move).Reverse();
        }

        public void Do(Move move)
        {
            MoveCommand moveCommand = new MoveCommand(move, _gameManager, _board);
            _moveStack.Push(moveCommand);
            moveCommand.Execute();
        }

        public void Undo()
        {
            _moveStack.BackPop()?.Undo();
        }

        public void Redo()
        {
            _moveStack.FrontPop()?.Execute();
        }

        public void Clear()
        {
            _moveStack.Clear();
        }
    }
}