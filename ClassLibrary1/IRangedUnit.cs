namespace Model
{
    public interface IRangedUnitInformation : IUnitInformation
    {
        static abstract int Range { get; }

        static abstract AmmunitionType AmmoType { get; }

        static abstract int Capacity { get; }
    }

    public interface IRangedUnit : IUnit
    {
        int AmmoLeft { get; }
    }
}
