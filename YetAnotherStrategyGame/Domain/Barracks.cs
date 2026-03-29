namespace YetAnotherStrategyGame.Domain
{
    class Barracks : IProductionBuilding
    {
        public int HP { get; private set; } = 1200;

        public int CostGold { get; private set; } = 20;

        public int CostFood { get; private set; } = 0;

        public int BuildTime { get; private set; } = 20;

        public Equipment EquipmentType { get; private set; } = Equipment.Sword;

        public int ProductionTime { get; private set; } = 10;

        public int ProductionCost { get; private set; } = 2;

        public int Capacity { get; private set; } = 5;
    }
}
