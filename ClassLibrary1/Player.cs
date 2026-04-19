namespace Model
{
    public class Player
    {
        public Game Game { get; }

        public string Name { get; private set; }

        public int Id { get; private set; }

        public Team Team { get; private set; }

        public TeamColor Color { get; private set; }

        public HashSet<IBuilding> OwnedBuildings { get; private set; }

        public HashSet<IUnit> OwnedUnits { get; private set; }

        public int Gold { get; private set; }

        public int Food { get; private set; }

        public BuildingType SelectedBuilding { get; private set; }

        public IUnit? SelectedUnit { get; private set; }

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

        public void Click(Cell cell)
        {
            var entity = cell.Entity;
            if (SelectedUnit != null)
            {
                SelectedUnit.ActUpon(cell);
                SelectedUnit = null;
            }
            else if (entity == null)
                TryBuild(cell);
            else
            {
                //Начать производить в здании
                throw new NotImplementedException();
            }
        }
        public void Select(Cell cell)
        {
            var entity = cell.Entity;
            if (entity is IUnit && entity.Owner == this)
                SelectedUnit = (IUnit)entity;
            else
                SelectedUnit = null;
        }

        public void TryBuild(Cell cell)
        {
            var buildingData = SelectedBuilding switch
            {
                BuildingType.Farm => (FarmInformation.CostGold, FarmInformation.CostFood, (Func<IBuilding>)(() => new Farm(cell, this))),
                BuildingType.Mine => (MineInformation.CostGold, MineInformation.CostFood, () => new Mine(cell, this)),
                BuildingType.Castle => (CastleInformation.CostGold, CastleInformation.CostFood, () => new Castle(cell, this)),
                BuildingType.Barracks => (BarracksInformation.CostGold, BarracksInformation.CostFood, () => new Barracks(cell, this)),
                BuildingType.CrossbowWorkshop => (CrossbowWorkshopInformation.CostGold, CrossbowWorkshopInformation.CostFood, () => new CrossbowWorkshop(cell, this)),
                BuildingType.CannonFactory => (CannonFactoryInformation.CostGold, CannonFactoryInformation.CostFood, () => new CannonFactory(cell, this)),
                _ => throw new ArgumentOutOfRangeException(nameof(SelectedBuilding), $"Тип {SelectedBuilding} не поддерживается")
            };
            var (costGold, costFood, createBuilding) = buildingData;
            if (Gold >= costGold && Food >= costFood)
            {
                Gold -= costGold;
                Food -= costFood;
                cell.PutEntity(createBuilding());
            }
        }

        public void BuySpawn(int spawnCost) => Food -= spawnCost;
    }
}
