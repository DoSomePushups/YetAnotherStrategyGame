namespace Model
{
    public interface IBuildingInformation : IEntityInformation
    {
        static abstract int CostGold { get; }

        static abstract int CostFood { get; }
    }

    public interface IBuilding : IEntity
    {

    }
}
