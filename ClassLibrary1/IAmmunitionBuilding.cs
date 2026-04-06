namespace Model
{
    public interface IAmmunitionBuilding : IBuilding
    {
        int AmmoProductionTime { get; }

        int AmmoCost { get; }

        int AmmoCapacity { get; }

        AmmunitionType AmmoType { get; }
    }
}
