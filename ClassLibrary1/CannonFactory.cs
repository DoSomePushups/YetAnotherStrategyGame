namespace ClassLibrary1
{
    public class CannonFactory : IProductionBuilding, IAmmunitionBuilding
    {
        public int HP { get; private set; } = 1800;

        public int CostGold { get; private set; } = 80;

        public int CostFood { get; private set; } = 0;

        public int BuildTime { get; private set; } = 50;

        public Equipment EquipmentType { get; private set; } = Equipment.Cannon;

        public int ProductionTime { get; private set; } = 30;

        public int ProductionCost { get; private set; } = 15;

        public int Capacity { get; private set; } = 3;

        public int AmmoProductionTime { get; private set; } = 10;

        public int AmmoCost { get; private set; } = 5;

        public int AmmoCapacity { get; private set; } = 6;

        public AmmunitionType AmmoType { get; private set; } = AmmunitionType.Cannonballs;
    }
}
