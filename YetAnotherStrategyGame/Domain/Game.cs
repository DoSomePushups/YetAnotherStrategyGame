using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherStrategyGame.Domain
{
    enum Team
    {
        First,
        Second
    }

    class Game
    {
        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }
    }

    class Player
    {
        public string Name { get; }
        public Team Team { get; }
    }

    class Field
    {
        public int Width { get; set; }

        public int Height { get; set; }



    }

    class Attack
    {

    }

    interface IUnit
    {

    }

    class Warrior : IUnit
    {

    }

    class Crossbowman : IUnit
    {

    }

    class Cannon : IUnit
    {

    }

    interface IBuilding
    {

    }

    class House : IBuilding
    {

    }

    class Farm : IBuilding
    {

    }

    class Mine : IBuilding
    {

    }

    class Barracks : IBuilding
    {

    }

    class CrossbowWorkshop : IBuilding
    {

    }

    class CannonFactory : IBuilding
    {

    }
}
