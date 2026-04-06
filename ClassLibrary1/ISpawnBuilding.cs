namespace Model
{
    public interface ISpawnBuilding : IBuilding
    {
        int SpawnTime { get; }

        int SpawnCost { get; }
    }
}
