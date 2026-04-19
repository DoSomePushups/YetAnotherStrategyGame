namespace Model
{
    public interface IProductionBuildingInformation : IBuildingInformation
    {
        static abstract Equipment EquipmentType { get; }

        static abstract int ProductionTime { get; }

        static abstract int ProductionCost { get; }

        static abstract int Capacity { get; }
    }

    public interface IProductionBuilding : IBuilding
    {
        //Производство снаряжения или аммунции (можно разделить на два метода)
        //public void Produce();
    }
}
