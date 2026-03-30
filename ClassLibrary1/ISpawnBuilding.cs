namespace ClassLibrary1
{
    public interface ISpawnBuilding : IBuilding
    {
        int SpawnTime { get; }

        int SpawnCost { get; }
    }
}
