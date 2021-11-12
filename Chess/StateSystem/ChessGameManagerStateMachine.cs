namespace Chess
{
    class ChessGameManagerStateMachine : StateMachine
    {
        private ChessGameManager _gameManager;

        public ChessGameManagerStateMachine(ChessGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public new void SetState<T>() where T : ChessGameManagerState, new()
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            CurrentState = new T();
            (CurrentState as ChessGameManagerState).GameManager = _gameManager;
            CurrentState.Enter();
        }
    }
}
