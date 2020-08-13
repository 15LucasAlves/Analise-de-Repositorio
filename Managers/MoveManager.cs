using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Managers
{
    class MoveManager
    {
        private Stack<Move> moves;

        public MoveManager()
        {
            moves = new Stack<Move>();
        }
    }
}