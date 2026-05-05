using System.Runtime.CompilerServices;
using System.Text;

namespace Model
{
    public class Robot0 : IRobot
    {
        public const int ControlRange = 5;
        public const int IntruderRange = 7;
        public const int EmergencyRange = 4;
        //public StringBuilder DebugBuilder = new StringBuilder();

        public Game.GameSession Session { get; }
        public Field Field => Session.GameField;
        public Player Player { get; }
        public Random Random { get; }

        public int HumanSpawns;

        private int PromotionNumber => HumanSpawns % 8;

        private class UnitMemory
        {
            public Cell? TargetCell;
            public IEntity? TargetEntity;
            public List<Cell> Path = new List<Cell>();
            public Role Role;
            public Intent Intent;
        }

        private Dictionary<IUnit, UnitMemory> Brain = new();
        private bool IsAttackingPhase = false;

        public Robot0(Game.GameSession session, Player player)
        {
            Session = session;
            Player = player;
            Random = new Random();
        }

        public void MakeMove()
        {
            // 1. Запись своих сущностей
            var farms = new List<Farm>();
            var mines = new List<Mine>();
            var barracks = new List<Barracks>();
            var crossbowWorkshops = new List<CrossbowWorkshop>();
            var cannonFactories = new List<CannonFactory>();
            var castles = new List<Castle>();
            var humans = new List<Human>();
            var warriors = new List<Warrior>();
            var crossbowmen = new List<Crossbowman>();
            var cannons = new List<Cannon>();

            foreach (var entity in Player.OwnedEntities)
            {
                if (entity is Farm farm)
                    farms.Add(farm);
                else if (entity is Mine mine)
                    mines.Add(mine);
                else if (entity is Barracks barrack)
                    barracks.Add(barrack);
                else if (entity is CrossbowWorkshop crossbowWorkshop)
                    crossbowWorkshops.Add(crossbowWorkshop);
                else if (entity is CannonFactory cannonFactory)
                    cannonFactories.Add(cannonFactory);
                else if (entity is Castle castle)
                    castles.Add(castle);
                else if (entity is Human human)
                    humans.Add(human);
                else if (entity is Warrior warrior)
                    warriors.Add(warrior);
                else if (entity is Crossbowman crossbowman)
                    crossbowmen.Add(crossbowman);
                else if (entity is Cannon cannon)
                    cannons.Add(cannon);
            }

            // Уборка мёртвых юнитов из памяти
            var deadUnits = Brain.Keys.Where(unit => !Player.OwnedEntities.Contains(unit)).ToList();
            foreach (var dead in deadUnits)
                Brain.Remove(dead);

            // 2. Постройка зданий
            if (farms.Count == 0)
            {
                Player.SelectStoreItem(BuildingType.Farm);
                Player.LeftClick(Field.Map[Field.Width - 1, 1]);
            }
            else if (mines.Count == 0)
            {
                Player.SelectStoreItem(BuildingType.Mine);
                Player.LeftClick(Field.Map[0, 1]);
            }
            else if (crossbowWorkshops.Count < 1)
                TryBuildBuilding(BuildingType.CrossbowWorkshop, 3);
            else if (farms.Count < 3)
                TryBuildBuilding(BuildingType.Farm, 0);
            else if (mines.Count < 4)
                TryBuildBuilding(BuildingType.Mine, 0);
            else if (farms.Count < 5)
                TryBuildBuilding(BuildingType.Farm, 0);
            else if (mines.Count < 5)
                TryBuildBuilding(BuildingType.Mine, 0);
            else if (barracks.Count < 1)
                TryBuildBuilding(BuildingType.Barracks, 3);
            else if (cannonFactories.Count < 1)
                TryBuildBuilding(BuildingType.CannonFactory, 3);
            else if (Random.Next(100) < 5) // 5% шанс построить дополнительную постройку каждый тик
            {
                if (farms.Count < 8)
                    TryBuildBuilding(BuildingType.Farm, 2);
                else if (mines.Count < 8)
                    TryBuildBuilding(BuildingType.Mine, 2);
                else if (barracks.Count < 2)
                    TryBuildBuilding(BuildingType.Barracks, 3);
                else if (crossbowWorkshops.Count < 2)
                    TryBuildBuilding(BuildingType.CrossbowWorkshop, 3);
                else if (cannonFactories.Count < 2)
                    TryBuildBuilding(BuildingType.CannonFactory, 3);
            }

            // 3. Производство снаряжения и аммуниции
            foreach (var barrack in barracks)
                if (barrack.ItemAmount == 0)
                    Player.LeftClick(barrack.Location);
            foreach (var crossbowWorkshop in crossbowWorkshops)
                if (crossbowWorkshop.AmmoAmount <= CrossbowWorkshopInformation.AmmoCapacity / 2 && crossbowmen.Count > 0
                    || crossbowWorkshop.AmmoAmount == 0)
                    Player.RightClick(crossbowWorkshop.Location);
                else if (crossbowWorkshop.ItemAmount == 0)
                    Player.LeftClick(crossbowWorkshop.Location);
            foreach (var cannonFactory in cannonFactories)
                if (cannonFactory.AmmoAmount <= CannonFactoryInformation.AmmoCapacity / 2)
                    Player.RightClick(cannonFactory.Location);
                else if (cannonFactory.ItemAmount == 0)
                    Player.LeftClick(cannonFactory.Location);


            // 4. Спавн людей
            if (humans.Count < 3 && Player.Food >= CastleInformation.SpawnCost)
            {
                foreach (var castle in castles)
                    if (castle.IsAvailable)
                    {
                        Player.LeftClick(castle.Location);
                        HumanSpawns++;
                    }
            }

            // 5. Сканирование опасностей
            var enemyEntities = Session.Players.First(player => player != Player).OwnedEntities.ToList();
            var enemyUnits = enemyEntities.Where(entity => entity is not Human).OfType<IUnit>().ToList();
            var intruders = enemyUnits.Where(enemy => enemy.Location.Y <= IntruderRange).ToList();
            var isDireEmergency = intruders.Any(enemy => enemy.Location.Y <= EmergencyRange);

            // 6. Фнализ своей армии
            var military = warriors.Cast<IUnit>().Concat(crossbowmen).Concat(cannons).ToList();
            var healthyMilitary = military.Where(militaryUnit => militaryUnit.HP == militaryUnit.GetMaxHP()).ToList();

            // Атака либо при более менее большой армии либо при перевесе сил
            if (healthyMilitary.Count >= 10 || healthyMilitary.Count >= enemyUnits.Count * 3)
                IsAttackingPhase = true;
            else if (military.Count <= 2)
                IsAttackingPhase = false;

            var allMyUnits = humans.Cast<IUnit>().Concat(military).ToList();

            var hasLeft = allMyUnits.Any(unit => Brain.ContainsKey(unit) && Brain[unit].Role == Role.HarvesterLeft);
            var hasRight = allMyUnits.Any(unit => Brain.ContainsKey(unit) && Brain[unit].Role == Role.HarvesterRight);

            // 7. Раздача ролей и целей
            foreach (var unit in allMyUnits)
            {
                if (!unit.IsAvailable)
                    continue;
                if (!Brain.ContainsKey(unit))
                    Brain[unit] = new UnitMemory();

                var unitMemory = Brain[unit];

                // Раздача ролей
                if (unit is Human)
                {
                    if (unitMemory.Role != Role.HarvesterLeft && unitMemory.Role != Role.HarvesterRight)
                    {
                        if (!hasLeft)
                        {
                            unitMemory.Role = Role.HarvesterLeft;
                            hasLeft = true;
                        }
                        else if (!hasRight)
                        {
                            unitMemory.Role = Role.HarvesterRight;
                            hasRight = true;
                        }
                        else
                            unitMemory.Role = Role.Recruit;
                    }
                }
                else
                    unitMemory.Role = Role.Military;

                // Раздача целей
                if (unitMemory.Role == Role.HarvesterLeft || unitMemory.Role == Role.HarvesterRight)
                    unitMemory.Intent = Intent.Harvest;
                else if (unitMemory.Role == Role.Recruit)
                {
                    unitMemory.Intent = Intent.Upgrade;
                }
                else if (unitMemory.Role == Role.Military)
                {
                    if (unit is IRangedUnit rangedUnit && rangedUnit.AmmoLeft == 0)
                        unitMemory.Intent = Intent.Resupply;
                    else if (unit.HP < unit.GetMaxHP() && !isDireEmergency)
                        unitMemory.Intent = Intent.Heal;
                    else if (intruders.Count > 0)
                        unitMemory.Intent = Intent.Defend;
                    else if (IsAttackingPhase)
                        unitMemory.Intent = Intent.Attack;
                    else
                        unitMemory.Intent = Intent.Rally;
                }
                ExecuteIntent(unit, unitMemory, enemyEntities);
            }
        }

        private void ExecuteIntent(IUnit controlledUnit, UnitMemory unitMemory, List<IEntity> enemyEntities)
        {
            switch (unitMemory.Intent)
            {
                case Intent.Heal:
                case Intent.Rally:
                    // Нахождение места где бы постоять
                    if (unitMemory.TargetCell == null || (!unitMemory.TargetCell.IsEmpty && unitMemory.TargetCell != controlledUnit.Location)
                        || unitMemory.TargetCell.Y > ControlRange)
                        unitMemory.TargetCell = GetRandomEmptyCellInTerritory();

                    if (unitMemory.TargetCell != null)
                    {
                        if (controlledUnit.Location.GetDistance(unitMemory.TargetCell) > 0)
                            ExecutePathOrAction(controlledUnit, unitMemory.TargetCell, 0, ref unitMemory.Path);
                    }
                    break;

                case Intent.Harvest:
                    IResourceBuilding? targetResourceBuilding = null;
                    // Левый собиратель собирает слева и наоборот
                    if (unitMemory.Role == Role.HarvesterLeft)
                        targetResourceBuilding = Player.OwnedEntities
                            .OfType<IResourceBuilding>()
                            .FirstOrDefault(resourceBuilding => resourceBuilding.IsAvailable && resourceBuilding.Location.X < Field.Width / 2)
                            ?? Player.OwnedEntities
                            .OfType<IResourceBuilding>()
                            .FirstOrDefault(resourceBuilding => resourceBuilding.Location.X < Field.Width / 2);
                    else if (unitMemory.Role == Role.HarvesterRight)
                        targetResourceBuilding = Player.OwnedEntities
                            .OfType<IResourceBuilding>()
                            .FirstOrDefault(resourceBuilding => resourceBuilding.IsAvailable && resourceBuilding.Location.X >= Field.Width / 2)
                            ?? Player.OwnedEntities
                            .OfType<IResourceBuilding>()
                            .FirstOrDefault(resourceBuilding => resourceBuilding.Location.X >= Field.Width / 2);

                    if (targetResourceBuilding != null
                        && (targetResourceBuilding.IsAvailable || controlledUnit.Location.GetDistance(targetResourceBuilding.Location) > 1))
                        ExecutePathOrAction(controlledUnit, targetResourceBuilding.Location, 1, ref unitMemory.Path);
                    break;

                case Intent.Upgrade:
                    IProductionBuilding? targetProductionBuilding = PromotionNumber < 4
                        ? Player.OwnedEntities
                        .OfType<Barracks>()
                        .FirstOrDefault(productionBuilding => productionBuilding.ItemAmount > 0)
                        : PromotionNumber < 6
                        ? Player.OwnedEntities
                        .OfType<CrossbowWorkshop>()
                        .FirstOrDefault(productionBuilding => productionBuilding.ItemAmount > 0)
                        : Player.OwnedEntities
                        .OfType<CannonFactory>()
                        .FirstOrDefault(productionBuilding => productionBuilding.ItemAmount > 0);
                    if (targetProductionBuilding == null)
                    {
                        targetProductionBuilding = Player.OwnedEntities
                        .OfType<IProductionBuilding>()
                        .FirstOrDefault(productionBuilding => productionBuilding.ItemAmount > 0);
                    }
                    if (targetProductionBuilding != null)
                        ExecutePathOrAction(controlledUnit, targetProductionBuilding.Location, 1, ref unitMemory.Path);
                    else
                    {
                        // Если негде получить специализацию, то просто отойти и ждать
                        if (unitMemory.TargetCell == null || (!unitMemory.TargetCell.IsEmpty && unitMemory.TargetCell != controlledUnit.Location)
                            || unitMemory.TargetCell.Y > ControlRange)
                            unitMemory.TargetCell = GetRandomEmptyCellInTerritory();
                        if (unitMemory.TargetCell != null && controlledUnit.Location.GetDistance(unitMemory.TargetCell) > 0)
                            ExecutePathOrAction(controlledUnit, unitMemory.TargetCell, 0, ref unitMemory.Path);
                    }
                    break;

                case Intent.Resupply:
                    IAmmunitionBuilding? ammoBuilding = null;
                    if (controlledUnit is Cannon)
                        ammoBuilding = Player.OwnedEntities.OfType<CannonFactory>().FirstOrDefault(factory => factory.AmmoAmount > 0);
                    else if (controlledUnit is Crossbowman)
                        ammoBuilding = Player.OwnedEntities.OfType<CrossbowWorkshop>().FirstOrDefault(workshop => workshop.AmmoAmount > 0);

                    if (ammoBuilding != null)
                        ExecutePathOrAction(controlledUnit, ammoBuilding.Location, 1, ref unitMemory.Path);
                    break;

                case Intent.Attack:
                case Intent.Defend:
                    var targets = unitMemory.Intent == Intent.Defend ? enemyEntities.Where(enemy => enemy.Location.Y <= 8).ToList() : enemyEntities;
                    if (targets.Count == 0)
                        break;
                    if (controlledUnit is Cannon)
                    {
                        var enemyBuildings = targets.Where(enemy => enemy is IBuilding).ToList();
                        unitMemory.TargetEntity = enemyBuildings.Count > 0
                            ? enemyBuildings.OrderBy(building => building.Location.GetDistance(controlledUnit.Location)).First()
                            : targets.OrderBy(enemy => enemy.Location.GetDistance(controlledUnit.Location)).First();
                    }
                    else
                    {
                        var enemyUnits = targets.Where(enemy => enemy is IUnit).ToList();
                        unitMemory.TargetEntity = enemyUnits.Count > 0
                            ? enemyUnits.OrderBy(unit => unit.Location.GetDistance(controlledUnit.Location)).First()
                            : targets.OrderBy(enemy => enemy.Location.GetDistance(controlledUnit.Location)).First();
                    }

                    if (unitMemory.TargetEntity != null)
                        ExecutePathOrAction(controlledUnit, unitMemory.TargetEntity.Location, GetRange(controlledUnit), ref unitMemory.Path);
                    break;
            }
        }

        private int GetRange(IUnit unit) => unit switch
        {
            Crossbowman => CrossbowmanInformation.Range,
            Cannon => CannonInformation.Range,
            _ => 1
        };

        private void TryBuildBuilding(BuildingType type, int preferredY)
        {
            var emptyCells = new List<Cell>();

            // Попытка поставить на предпочтительной Y координате
            for (var x = 0; x < Field.Width; x++)
                if (x != 5 && Field.Map[x, preferredY].IsEmpty)
                    emptyCells.Add(Field.Map[x, preferredY]);

            // При невозможности строим где можем
            if (emptyCells.Count == 0)
            {
                for (var y = 0; y <= 3; y++)
                {
                    for (var x = 0; x < Field.Width; x++)
                    {
                        if (x != 5 && Field.Map[x, y].IsEmpty)
                            emptyCells.Add(Field.Map[x, y]);
                    }
                }
            }

            if (emptyCells.Count > 0)
            {
                Player.SelectStoreItem(type);
                Player.LeftClick(emptyCells[Random.Next(emptyCells.Count)]);
            }
        }

        private Cell? GetRandomEmptyCellInTerritory()
        {
            var emptyCells = new List<Cell>();
            for (var x = 0; x < Field.Width; x++)
            {
                for (var y = 2; y <= ControlRange; y++)
                {
                    if (x != 5 && Field.Map[x, y].IsEmpty)
                        emptyCells.Add(Field.Map[x, y]);
                }
            }
            return emptyCells.Count > 0 ? emptyCells[Random.Next(emptyCells.Count)] : null;
        }

        private void ExecutePathOrAction(IUnit unit, Cell targetCell, int actionRange, ref List<Cell> currentPath)
        {
            if (targetCell == null)
                return;

            // Цель в области атаки
            if (unit.Location.GetDistance(targetCell) <= actionRange)
            {
                // 0 означает, что действие выполнять не надо
                if (actionRange > 0)
                {
                    Player.LeftClick(unit.Location);
                    Player.LeftClick(targetCell);
                }
                currentPath.Clear();
                return;
            }

            // Пересчет пути, если что-то пошло не так
            if (currentPath.Count == 0 || (!currentPath[0].IsEmpty && currentPath[0] != targetCell))
                currentPath = CalculatePath(unit.Location, targetCell);

            // Шаг
            if (currentPath.Count > 0)
            {
                var nextStep = currentPath[0];

                // Путь заблокировала дружеская сущность
                if (!nextStep.IsEmpty && nextStep.Entity?.Owner == Player && nextStep != targetCell)
                {
                    currentPath.Clear();
                    return;
                }

                currentPath.RemoveAt(0);
                Player.LeftClick(unit.Location);
                Player.LeftClick(nextStep);
            }
        }

        private List<Cell> CalculatePath(Cell start, Cell target)
        {
            var queue = new Queue<Cell>();
            var cameFrom = new Dictionary<Cell, Cell?>();

            queue.Enqueue(start);
            cameFrom[start] = null;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == target)
                    break;

                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        var newX = current.X + i;
                        var newY = current.Y + j;

                        if (Field.CheckCellExist(newX, newY))
                        {
                            var neighbor = Field.Map[newX, newY];

                            // Кликаем только на пустые клетки для передвижения или на цель или на врага на пути, чтобы атаковать
                            var isPassable = neighbor.IsEmpty || neighbor == target || (neighbor.Entity != null && neighbor.Entity.Owner != Player);

                            if (!cameFrom.ContainsKey(neighbor) && isPassable)
                            {
                                cameFrom[neighbor] = current;
                                queue.Enqueue(neighbor);
                            }
                        }
                    }
                }
            }

            if (!cameFrom.ContainsKey(target))
                return new List<Cell>();

            var path = new List<Cell>();
            var pathPart = cameFrom[target];
            while (pathPart != null && pathPart != start)
            {
                path.Add(pathPart);
                pathPart = cameFrom[pathPart];
            }
            path.Reverse();
            path.Add(target);
            return path;
        }
    }
}