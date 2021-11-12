using Microsoft.Xna.Framework;

namespace Chess
{
    class StateMachine
    {
        public State CurrentState { get; protected set; }

        public void SetState<T>() where T : State, new()
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            CurrentState = new T();
            CurrentState.Enter();
        }

        public void Update(GameTime gameTime)
        {
            CurrentState?.Update(gameTime);
        }
    }
}
