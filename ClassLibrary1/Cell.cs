namespace ClassLibrary1
{
    public class Cell
    {
        public IEntity Entity { get; private set; } = null;

        public bool IsEmpty => Entity == null;
    }
}
