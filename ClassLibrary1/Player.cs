namespace Model
{
    public class Player
    {
        public Game.GameSession GameSession { get; }

        public string Name { get; private set; }

        public int Id { get; private set; }

        public Team Team { get; private set; }

        public TeamColor Color { get; private set; }

        public HashSet<IBuilding> OwnedBuildings { get; private set; }

        public HashSet<IUnit> OwnedUnits { get; private set; }

        public int Gold { get; private set; }

        public int Food { get; private set; }

        public Action<int, int> StatsChanged;

        public BuildingType? SelectedBuilding { get; private set; }

        public IUnit? SelectedUnit { get; private set; }

        public Player(Game.GameSession session, string name, int id)
        {
            GameSession = session;
            Name = name;
            Id = id;
            Team = (Team)id;
            Color = (TeamColor)id;
            OwnedBuildings = new HashSet<IBuilding>();
            OwnedUnits = new HashSet<IUnit>();
            Gold = 100;
            Food = 50;
            SelectedBuilding = null;
        }

        public void Click(Cell cell)
        {
            var entity = cell.Entity;
            var owner = entity?.Owner;
            if (SelectedUnit != null)
            {
                SelectedUnit.ActUpon(cell);
                SelectedUnit = null;
            }
            else if (entity == null && SelectedBuilding != null)
                TryBuild(cell);
            else if (entity is ISpawnBuilding spawnBuilding)
                spawnBuilding.TrySpawn();
            else
            {
                //Начать производить в здании
                throw new NotImplementedException();
            }
        }

        public void SelectStoreItem(BuildingType? buildingType)
        {
            if (SelectedBuilding != buildingType)
                SelectedBuilding = buildingType;
            else
                SelectedBuilding = null;
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
                StatsChanged?.Invoke(Food, Gold);
                cell.PutEntity(createBuilding());
            }
        }

        public void BuySpawn(int spawnCost)
        {
            Food -= spawnCost;
            StatsChanged?.Invoke(Food, Gold);
        }
    }
}
