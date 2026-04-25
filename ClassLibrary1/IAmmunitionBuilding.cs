namespace Model
{
    public interface IAmmunitionBuildingInformation : IBuildingInformation
    {
        static abstract int AmmoCapacity { get; }

        static abstract AmmunitionType AmmoType { get; }
    }

    public interface IAmmunitionBuilding : IBuilding
    {
        public int AmmoAmount { get; }

        public void ProduceAmmo();
    }
}
