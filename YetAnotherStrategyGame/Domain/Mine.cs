namespace YetAnotherStrategyGame.Domain
{
    class Mine : IResourceBuilding
    {
        public int HP { get; private set; } = 800;

        public int CostGold { get; private set; } = 10;

        public int CostFood { get; private set; } = 0;

        public int BuildTime { get; private set; } = 20;

        public int PassiveProductionTime { get; private set; } = 6;

        public PassiveResourceType PassiveResourceType { get; private set; } = PassiveResourceType.Gold;
    }
}
