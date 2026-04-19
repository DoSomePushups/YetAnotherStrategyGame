namespace Model
{
    public class BarracksInformation : IProductionBuildingInformation
    {
        public static int MaxHP { get; } = 1200;

        public static int CostGold { get; } = 20;

        public static int CostFood { get; } = 0;

        public static int BuildTime { get; } = 20;

        public static Equipment EquipmentType { get; } = Equipment.Sword;

        public static int ProductionTime { get; } = 10;

        public static int ProductionCost { get; } = 2;

        public static int Capacity { get; } = 5;
    }

    public class Barracks : IProductionBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Barracks(Cell location, Player owner)
        {
            HP = 1200;
            UnactionTime = 0;
            Location = location;
            Owner = owner;
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = BarracksInformation.MaxHP;
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
