namespace Model
{
    public interface IResourceBuilding : IBuilding
    {
        int PassiveProductionTime { get; }

        PassiveResourceType PassiveResourceType { get; }
    }
}
