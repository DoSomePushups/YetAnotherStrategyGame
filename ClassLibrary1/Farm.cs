namespace ClassLibrary1
{
    public class Farm : IResourceBuilding
    {
        public int HP { get; private set; } = 150;

        public int CostGold { get; private set; } = 5;

        public int CostFood { get; private set; } = 5;

        public int BuildTime { get; private set; } = 15;

        public int PassiveProductionTime { get; private set; } = 5;

        public PassiveResourceType PassiveResourceType { get; private set; } = PassiveResourceType.Food;
    }
}
