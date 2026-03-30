namespace ClassLibrary1
{
    public interface IRangedUnit : IUnit
    {
        int Range { get; }

        AmmunitionType AmmoType { get; }
    }
}
