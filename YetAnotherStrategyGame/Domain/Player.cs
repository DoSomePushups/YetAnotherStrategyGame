using System.Collections.Generic;

namespace YetAnotherStrategyGame.Domain
{
    class Player
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        public Team Team { get; private set; }

        public Color Color { get; private set; }

        public HashSet<IBuilding> OwnedBuildings { get; private set; }

        public HashSet<IUnit> OwnedUnits { get; private set; }

        public int Gold { get; private set; }

        public int Food { get; private set; }

    }
}
