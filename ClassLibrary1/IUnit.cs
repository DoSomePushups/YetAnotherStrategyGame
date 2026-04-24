namespace Model
{
    public interface IUnitInformation : IEntityInformation
    {
        static abstract int Damage { get; }

        static abstract UnitType Type { get; }

        static abstract int AttackCD { get; }

        static abstract int MoveCD { get; }
    }

    public interface IUnit : IEntity
    {
        public bool HasMoved { get; }

        public bool HasAttacked { get; }

        public void ActUpon(Cell actionObject);
    }
}
