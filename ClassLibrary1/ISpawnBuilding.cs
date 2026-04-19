namespace Model
{
    public interface ISpawnBuildingInformation : IBuildingInformation
    {
        static abstract int SpawnTime { get; }

        static abstract int SpawnCost { get; }
    }

    public interface ISpawnBuilding : IBuilding
    {
        public void TrySpawn();
    }
}
