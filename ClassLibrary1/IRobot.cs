using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Model
{
    public interface IRobot
    {
        public Game.GameSession Session { get; }
        public Player Player { get; }
        public Random Random { get; }
        public void MakeMove();
    }
}