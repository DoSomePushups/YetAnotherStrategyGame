namespace Model
{
    public interface IBuildingInformation : IEntityInformation
    {
        static abstract int CostGold { get; }

        static abstract int CostFood { get; }

        static abstract int BuildTime { get; }
    }

    public interface IBuilding : IEntity
    {
        
    }
}
