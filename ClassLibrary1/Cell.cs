namespace Model
{
    public class Cell
    {
        public readonly int X;

        public readonly int Y;

        public IEntity? Entity { get; private set; }

        public event Action<Cell> CellChanged;

        public bool IsEmpty => Entity == null;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Entity = null;
        }

        public void PutEntity(IEntity entity)
        {
            Entity = entity;
            CellChanged?.Invoke(this);
        }

        public void RemoveEntity()
        {
            Entity = null;
            CellChanged?.Invoke(this);
        }

        public int GetDistance(Cell cell)
        {
            var offsetX = Math.Abs(cell.X - X);
            var offsetY = Math.Abs(cell.Y - Y);
            return Math.Max(offsetX, offsetY);
        }
    }
}
