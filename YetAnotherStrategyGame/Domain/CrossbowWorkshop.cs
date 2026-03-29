namespace YetAnotherStrategyGame.Domain
{
    class CrossbowWorkshop : IProductionBuilding, IAmmunitionBuilding
    {
        public int HP { get; private set; } = 1200;

        public int CostGold { get; private set; } = 50;

        public int CostFood { get; private set; } = 0;

        public int BuildTime { get; private set; } = 20;

        public Equipment EquipmentType { get; private set; } = Equipment.Crossbow;

        public int ProductionTime { get; private set; } = 10;

        public int ProductionCost { get; private set; } = 2;

        public int Capacity { get; private set; } = 5;

        public int AmmoProductionTime { get; private set; } = 3;

        public int AmmoCost { get; private set; } = 2;

        public int AmmoCapacity { get; private set; } = 18;

        public AmmunitionType AmmoType { get; private set; } = AmmunitionType.Arrows;
    }
}
