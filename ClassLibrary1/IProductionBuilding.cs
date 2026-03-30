namespace ClassLibrary1
{
    public interface IProductionBuilding : IBuilding
    {
        Equipment EquipmentType { get; }

        int ProductionTime { get; }

        int ProductionCost { get; }

        int Capacity { get; }
    }
}
