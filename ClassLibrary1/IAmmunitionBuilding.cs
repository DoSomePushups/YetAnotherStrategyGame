namespace Model
{
    public interface IAmmunitionBuildingInformation : IBuildingInformation
    {
        static abstract int AmmoProductionTime { get; }

        static abstract int AmmoCost { get; }

        static abstract int AmmoCapacity { get; }

        static abstract AmmunitionType AmmoType { get; }
    }

    public interface IAmmunitionBuilding : IBuilding
    {
        
    }
}
