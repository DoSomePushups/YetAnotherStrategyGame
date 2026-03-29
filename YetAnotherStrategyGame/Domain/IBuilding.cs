namespace YetAnotherStrategyGame.Domain
{
    interface IBuilding : IEntity
    {
        int CostGold { get; }

        int CostFood { get; }

        int BuildTime { get; }
    }
}
