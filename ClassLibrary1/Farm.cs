namespace Model
{
    public class FarmInformation : IResourceBuildingInformation
    {
        public static int MaxHP { get; } = 150;

        public static int CostGold { get; } = 5;

        public static int CostFood { get; } = 5;

        public static int BuildTime { get; } = 15;

        public static int PassiveProductionTime { get; } = 5;

        public static PassiveResourceType PassiveResourceType { get; } = PassiveResourceType.Food;
    }

    public class Farm : IResourceBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public event Action<Cell> HpChanged;

        public Farm(Cell location, Player owner)
        {
            HP = 150;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = FarmInformation.MaxHP;
            if (newHealth < maxHP)
                HP = newHealth;
            else
                HP = maxHP;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            HpChanged?.Invoke(Location);
            if (HP <= 0)
                Die();
        }

        public void Die() => Location.RemoveEntity();
    }
}
