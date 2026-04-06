namespace Model
{
    public class Cell
    {
        public readonly int X;

        public readonly int Y;

        public IEntity? Entity { get; private set; } = null;

        public bool IsEmpty => Entity == null;

        public Cell (int x, int y)
        {
            X = x;
            Y = y;
        }

        public void PutEntity(IEntity entity) => Entity = entity;

        public void RemoveEntity() => Entity = null;

        public int GetDistance(Cell cell)
        {
            var offsetX = Math.Abs(cell.X - X);
            var offsetY = Math.Abs(cell.Y - Y);
            return Math.Max(offsetX, offsetY);
        }
    }
}
