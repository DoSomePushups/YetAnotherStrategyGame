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
            var spawnPlace = Owner.Game.Session.GameField.Map[Location.X + 1, Location.Y];
            var spawnCost = CastleInformation.SpawnCost;
            if (Owner.Food >= spawnCost && spawnPlace.Entity == null)
            {
                Owner.BuySpawn(spawnCost);
                spawnPlace.PutEntity(new Human(spawnPlace, Owner));
            }
        }

    }
}
