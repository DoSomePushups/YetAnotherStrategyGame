namespace YetAnotherStrategyGame.Domain
{
    interface ISpawnBuilding : IBuilding
    {
        int SpawnTime { get; }

        int SpawnCost { get; }
    }
}
