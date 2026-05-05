namespace Model
{
    public class Player
    {
        public Game.GameSession GameSession { get; }

        public IRobot Robot { get; }

        public string Name { get; private set; }

        public int Id { get; private set; }

        public Team Team { get; private set; }

        public TeamColor Color { get; private set; }

        public HashSet<IEntity> OwnedEntities { get; private set; }

        public int Gold { get; private set; }

        public int Food { get; private set; }

        public Action<int, int> StatsChanged;

        public BuildingType? SelectedBuilding { get; private set; }

        public IUnit? SelectedUnit { get; private set; }

        public Player(Game.GameSession session, string name, int id, int RobotNumber)
        {
            GameSession = session;
            Name = name;
            Id = id;
            Team = (Team)id;
            Color = (TeamColor)id;
            OwnedEntities = new();
            Gold = 100;
            Food = 50;
            SelectedBuilding = null;
            if (RobotNumber == 0)
                Robot = new Robot0(GameSession, this);
            else if (RobotNumber == 1)
                Robot = new Robot1(GameSession, this);
        }

        public void LeftClick(Cell cell)
        {
            var entity = cell.Entity;
            var owner = entity?.Owner;
            var fieldHeight = GameSession.GameField.Height;
            var onTerritory = Team == Team.First ? cell.Y >= fieldHeight - (fieldHeight / 3) : cell.Y <= fieldHeight / 3;

            if (entity != null && this == owner && !entity.IsAvailable)
                return;

            if (SelectedUnit != null)
            {
                if (SelectedUnit.HP > 0)
                    SelectedUnit.ActUpon(cell);
                SelectedUnit = null;
            }
            else if (entity == null || owner == this)
            {
                if (entity is ISpawnBuilding spawnBuilding)
                    spawnBuilding.TrySpawn();
                else if (entity is IUnit unit)
                    SelectedUnit = unit;
                else if (entity == null && SelectedBuilding != null && onTerritory)
                    TryBuild(cell);
                else if (entity is IProductionBuilding productionBuilding)
                    productionBuilding.Produce();
            }
        }

        public void RightClick(Cell cell)
        {
            var entity = cell.Entity;
            var owner = entity?.Owner;
            if (owner == this && entity is IAmmunitionBuilding ammunitionBuilding && entity.IsAvailable)
                ammunitionBuilding.ProduceAmmo();
        }

        public void MiddleClick(Cell cell)
        {
            var entity = cell.Entity;
            var owner = entity?.Owner;
            if (owner == this && entity != null)
                entity.Die();
        }

        public void SelectStoreItem(BuildingType? buildingType)
        {
            if (SelectedBuilding != buildingType)
                SelectedBuilding = buildingType;
            else
                SelectedBuilding = null;
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
                var building = createBuilding();
                cell.PutEntity(building);
                GameSession.OnTick += building.HandleTick;
                SelectedBuilding = null;
                OwnedEntities.Add(building);
            }
        }

        public bool TryBuy(int foodCost, int goldCost)
        {
            if (Food >= foodCost && Gold >= goldCost)
            {
                Food -= foodCost;
                Gold -= goldCost;
                StatsChanged?.Invoke(Food, Gold);
                return true;
            }
            return false;
        }

        public void GetResources(int food, int gold)
        {
            Food += food;
            Gold += gold;
            StatsChanged?.Invoke(Food, Gold);
        }
    }
}
