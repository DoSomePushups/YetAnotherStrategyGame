namespace Model
{
    public class Castle : ISpawnBuilding
    {
        public int MaxHP { get; private set; }

        public int HP { get; private set; } = 2000;

        public int CostGold { get; private set; } = 120;

        public int CostFood { get; private set; } = 10;

        public int BuildTime { get; private set; } = 50;

        public int SpawnTime { get; private set; } = 9;

        public int SpawnCost { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Castle(Cell location, Player owner)
        {
            MaxHP = 2000;
            HP = 2000;
            CostGold = 120;
            CostFood = 10;
            BuildTime = 50;
            SpawnTime = 9;
            SpawnCost = 10;
            Location = location;
            Owner = owner;
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            if (newHealth < MaxHP)
                HP = newHealth;
            else
                HP = MaxHP;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP <= 0)
                Die();
        }

        public void Die() => Location.RemoveEntity();
    }
}
