namespace Model
{
    public interface IRangedUnit : IUnit
    {
        int Range { get; }

        AmmunitionType AmmoType { get; }

        int AmmoLeft { get; }

        int Capacity { get; }
    }
}
