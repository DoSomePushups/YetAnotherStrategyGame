using System;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherStrategyGame.Domain
{

    class Game
    {
        public State GameState { get; private set; }

        public Player FirstPlayer { get; private set; }

        public Player SecondPlayer { get; private set; }
    }
}
