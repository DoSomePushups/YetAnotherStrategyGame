namespace ClassLibrary1
{
    public interface IBuilding : IEntity
    {
        int CostGold { get; }

        int CostFood { get; }

        int BuildTime { get; }
    }
}
