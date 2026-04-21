namespace Model
{
    public class CastleInformation : ISpawnBuildingInformation
    {
        public static int MaxHP { get; } = 2000;

        public static int CostGold { get; } = 120;

        public static int CostFood { get; } = 10;

        public static int BuildTime { get; } = 50;

        public static int SpawnTime { get; } = 9;

        public static int SpawnCost { get; } = 10;
    }

    public class Castle : ISpawnBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Castle(Cell location, Player owner)
        {
            HP = 2000;
            UnactionTime = 0;
            Location = location;
            Owner = owner;
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = CastleInformation.MaxHP;
            if (newHealth < maxHP)
                HP = newHealth;
            else
                HP = maxHP;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP <= 0)
                Die();
        }

        public void Die() => Location.RemoveEntity();

        public void TrySpawn()
        {
            var spawnPoints = new List<Cell>(8);
            var field = Owner.GameSession.GameField;
            for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                {
                    var x = Location.X + i;
                    var y = Location.Y + j;
                    if (field.CheckCellExist(x, y))
                    {
                        var cell = field.Map[x, y];
                        if (cell.IsEmpty)
                            spawnPoints.Add(cell);
                    }
                }
            var spawnPoint = spawnPoints.FirstOrDefault();
            var spawnCost = CastleInformation.SpawnCost;
            if (spawnPoint != null && Owner.Food >= spawnCost)
            {
                Owner.BuySpawn(spawnCost);
                spawnPoint.PutEntity(new Human(spawnPoint, Owner));
            }
        }

    }
}
