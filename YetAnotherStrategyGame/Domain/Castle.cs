namespace YetAnotherStrategyGame.Domain
{
    class Castle : ISpawnBuilding
    {
        public int HP { get; private set; } = 2000;

        public int CostGold { get; private set; } = 120;

        public int CostFood { get; private set; } = 10;

        public int BuildTime { get; private set; } = 50;

        public int SpawnTime { get; private set; } = 9;

        public int SpawnCost => 10;
    }
}
