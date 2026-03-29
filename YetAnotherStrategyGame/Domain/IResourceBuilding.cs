namespace YetAnotherStrategyGame.Domain
{
    interface IResourceBuilding : IBuilding
    {
        int PassiveProductionTime { get; }

        PassiveResourceType PassiveResourceType { get; }
    }
}
