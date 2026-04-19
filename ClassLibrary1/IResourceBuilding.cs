namespace Model
{
    public interface IResourceBuildingInformation : IBuildingInformation
    {
        static abstract int PassiveProductionTime { get; }

        static abstract PassiveResourceType PassiveResourceType { get; }
    }

    public interface IResourceBuilding : IBuilding
    {
        
    }
}
