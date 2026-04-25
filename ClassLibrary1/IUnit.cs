namespace Model
{
    public interface IUnitInformation : IEntityInformation
    {
        static abstract int Damage { get; }

        static abstract UnitType Type { get; }
    }

    public interface IUnit : IEntity
    {
        public void ActUpon(Cell actionObject);
    }
}
