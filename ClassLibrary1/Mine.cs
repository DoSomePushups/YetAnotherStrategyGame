namespace Model
{
    public class MineInformation : IResourceBuildingInformation
    {
        public static int MaxHP { get; } = 800;

        public static int CostGold { get; } = 10;

        public static int CostFood { get; } = 0;

        public static int BuildTime { get; } = 20;

        public static int PassiveProductionTime { get; } = 6;

        public static PassiveResourceType PassiveResourceType { get; } = PassiveResourceType.Gold;
    }

    public class Mine : IResourceBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Mine(Cell location, Player owner)
        {
            HP = 800;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = MineInformation.MaxHP;
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
    }
}
