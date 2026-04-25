namespace Model
{
    public interface IProductionBuildingInformation : IBuildingInformation
    {
        static abstract Equipment EquipmentType { get; }

        static abstract int ProductionCost { get; }

        static abstract int Capacity { get; }
    }

    public interface IProductionBuilding : IBuilding
    {
        public int ItemAmount { get; }

        public void Produce();

        public bool TryTrain(Cell location);
    }
}
