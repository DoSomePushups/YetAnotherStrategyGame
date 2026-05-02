namespace Model
{
    public interface IRobot
    {
        public Game.GameSession Session { get; }

        public Player Player { get; }

        public Random Random { get; }

        public void MakeMove();
    }

    public class Robot0 : IRobot
    {
        public Game.GameSession Session { get; }

        public Field Field => Session.GameField;

        public Player Player { get; }

        public Random Random { get; }

        public int Food => Player.Food;

        public int Gold => Player.Gold;

        public int FarmCostFood => FarmInformation.CostFood;

        public int FarmCostGold => FarmInformation.CostGold;

        public int MineCostFood => MineInformation.CostFood;

        public int MineCostGold => MineInformation.CostGold;

        private int FarmCount;

        private int MineCount;

        private int HumanCount;

        private Queue<Action> FutureMoves { get; }

        public Robot0(Game.GameSession session, Player player)
        {
            Session = session;
            Player = player;
            Random = new Random();
            FutureMoves = new();
        }

        public void MakeMove()
        {
            foreach (var entity in Player.OwnedEntities)
            {
                switch (entity)
                {
                    case Farm:
                        FarmCount++;
                        break;
                    case Mine:
                        MineCount++;
                        break;
                    case Human human:
                        HumanCount++;
                        HandleHuman(human);
                        break;
                    case Castle castle:
                        FutureMoves.Enqueue(() => HandleCastle(castle));
                        break;
                }
            }
            if (FarmCount < 3 && Food >= FarmCostFood && Gold >= FarmCostGold)
                PlaceFarm();
            else if (MineCount < 4 && Food >= MineCostFood && Gold >= MineCostGold)
                PlaceMine();
            while (FutureMoves.Count > 0)
                FutureMoves.Dequeue()();
            ResetCount();
        }

        private void PlaceFarm()
        {
            SelectStoreItem(BuildingType.Farm);
            LeftClick(Field.Map[Random.Next(0, Field.Width), 0]);
        }

        private void PlaceMine()
        {
            SelectStoreItem(BuildingType.Mine);
            LeftClick(Field.Map[Random.Next(0, Field.Width), 0]);
        }

        private void HandleCastle(Castle castle)
        {
            if (castle.IsAvailable && HumanCount < 2 && Food >= CastleInformation.SpawnCost)
                LeftClick(castle.Location);
        }

        private void HandleHuman(Human human)
        {
            var nextStep = FindMove(Field, human.Location, EntityType.Farm);
            if (nextStep != null)
            {
                LeftClick(human.Location);
                LeftClick(nextStep);
            }
        }

        public static Cell? FindMove(Field field, Cell start, EntityType soughtObject)
        {
            var cellsToVisit = new Queue<SinglyLinkedList<Cell>>();
            var visited = new HashSet<Cell>();
            visited.Add(start);
            cellsToVisit.Enqueue(new SinglyLinkedList<Cell>(start));
            var nextStep = DoBFS(field, cellsToVisit, visited, soughtObject);
            while (nextStep?.Previous != null)
                nextStep = nextStep.Previous;
            return nextStep?.Value;
        }

        private static SinglyLinkedList<Cell>? DoBFS(Field field, Queue<SinglyLinkedList<Cell>> cellsToVisit, HashSet<Cell> visited, EntityType soughtObject)
        {
            while (cellsToVisit.Count > 0)
            {
                var cell = cellsToVisit.Dequeue();
                for (var i = -1; i <= 1; i++)
                    for (var j = -1; j <= 1; j++)
                        if (field.CheckCellExist(cell.Value.X + i, cell.Value.Y + j))
                        {
                            var newCell = field.Map[cell.Value.X + i, cell.Value.Y + j];
                            if (newCell.Entity?.Type == soughtObject && cell.Value.Entity.IsAvailable)
                                return new SinglyLinkedList<Cell>(newCell, cell);
                            if (visited.Contains(newCell) || newCell.Entity != null)
                                continue;
                            var neighborCell = new SinglyLinkedList<Cell>(newCell, cell);
                            cellsToVisit.Enqueue(neighborCell);
                            visited.Add(newCell);
                        }
            }
            return null;
        }

        private void ResetCount()
        {
            FarmCount = 0;
            MineCount = 0;
            HumanCount = 0;
        }

        private void SelectStoreItem(BuildingType item) => Player.SelectStoreItem(item);

        private void LeftClick(Cell cell) => Player.LeftClick(cell);

        private void RightClick(Cell cell) => Player.RightClick(cell);

        private void MiddleClick(Cell cell) => Player.MiddleClick(cell);
    }

    public class SinglyLinkedList<T>
    {
        public T Value;

        public SinglyLinkedList<T>? Previous;

        public SinglyLinkedList(T item, SinglyLinkedList<T>? previous = null)
        {
            Value = item;
            Previous = previous;
        }
    }
}
