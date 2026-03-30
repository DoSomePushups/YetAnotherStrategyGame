namespace ClassLibrary1
{
    public class Player
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        public Team Team { get; private set; }

        public TeamColor Color { get; private set; }

        public HashSet<IBuilding> OwnedBuildings { get; private set; }

        public HashSet<IUnit> OwnedUnits { get; private set; }

        public int Gold { get; private set; }

        public int Food { get; private set; }

        public Player(string name, int id)
        {
            Name = name;
            Id = id;
            Team = (Team)id;
            Color = (TeamColor)id;
            OwnedBuildings = new HashSet<IBuilding>();
            OwnedUnits = new HashSet<IUnit>();
            Gold = 0;
            Food = 0;
        }
    }
}
