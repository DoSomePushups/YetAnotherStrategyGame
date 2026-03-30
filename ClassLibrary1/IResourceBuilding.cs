namespace ClassLibrary1
{
    public interface IResourceBuilding : IBuilding
    {
        int PassiveProductionTime { get; }

        PassiveResourceType PassiveResourceType { get; }
    }
}
