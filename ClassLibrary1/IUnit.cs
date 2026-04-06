namespace Model
{
    public interface IUnit : IEntity
    {
        int Damage { get; }

        UnitType Type { get; }

        int AttackCD { get; }

        int MoveCD { get; }

        public void ActUpon(Cell actionObject);
    }
}
