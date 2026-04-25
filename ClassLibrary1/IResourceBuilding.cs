namespace Model
{
    public interface IResourceBuildingInformation : IBuildingInformation
    {
        static abstract PassiveResourceType PassiveResourceType { get; }

        static abstract int ResourceProductionQuantity { get; }
    }

    public interface IResourceBuilding : IBuilding
    {
        public void Collect();
    }
}
