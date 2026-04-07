namespace Model
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

        public BuildingType SelectedBuilding { get; private set; }

        public IUnit SelectedUnit { get; private set; }

        public Player(string name, int id)
        {
            Name = name;
            Id = id;
            Team = (Team)id;
            Color = (TeamColor)id;
            OwnedBuildings = new HashSet<IBuilding>();
            OwnedUnits = new HashSet<IUnit>();
            Gold = 100;
            Food = 50;
            SelectedBuilding = BuildingType.Farm;
        }

        public void TryBuild(Cell cell)
        {
            IBuilding building = SelectedBuilding switch
            {
                BuildingType.Farm => new Farm(cell, this),
                BuildingType.Mine => new Mine(cell, this),
                BuildingType.Castle => new Castle(cell, this),
                BuildingType.Barracks => new Barracks(cell, this),
                BuildingType.CrossbowWorkshop => new CrossbowWorkshop(cell, this),
                BuildingType.CannonFactory => new CannonFactory(cell, this)
            };
            var costGold = building.CostGold;
            var costFood = building.CostFood;
            if (Gold >= building.CostGold && Food >= building.CostFood)
            {
                Gold -= costGold;
                Food -= costFood;
                cell.PutEntity(building);
            }
        }


    }
}
