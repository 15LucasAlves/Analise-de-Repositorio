using System;

namespace Chess
{
    class TurnManager
    {
        public int TurnIndex { get; private set; } = 0;
        public Team TurnTeam { get; private set; } = Team.White;


        public event Action<Team> OnTurnChange;


        public void NextTurn()
        {
            switch (TurnTeam)
            {
                case Team.White:
                    {
                        TurnTeam = Team.Black;
                        break;
                    }
                case Team.Black:
                    {
                        TurnTeam = Team.White;
                        break;
                    }
            }

            TurnIndex++;
            OnTurnChange?.Invoke(TurnTeam);
        }

        public void PreviousTurn()
        {
            switch (TurnTeam)
            {
                case Team.White:
                    {
                        TurnTeam = Team.Black;
                        break;
                    }
                case Team.Black:
                    {
                        TurnTeam = Team.White;
                        break;
                    }
            }

            TurnIndex--;
            OnTurnChange?.Invoke(TurnTeam);
        }

        public void SetTurn(int turnIndex, Team team)
        {
            TurnIndex = turnIndex;
            TurnTeam = team;
            OnTurnChange?.Invoke(TurnTeam);
        }
    }
}
