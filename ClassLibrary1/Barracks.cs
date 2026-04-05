namespace ClassLibrary1
{
    public class Barracks : IProductionBuilding
    {
        public int MaxHP { get; private set; }

        public int HP { get; private set; }

        public int CostGold { get; private set; }

        public int CostFood { get; private set; }

        public int BuildTime { get; private set; }

        public Equipment EquipmentType { get; private set; }

        public int ProductionTime { get; private set; }

        public int ProductionCost { get; private set; }

        public int Capacity { get; private set; }

        public Cell Location { get; private set; }

        public Barracks(Cell location)
        {
            MaxHP = 1200;
            HP = 1200;
            CostGold = 20;
            CostFood = 0;
            BuildTime = 20;
            EquipmentType = Equipment.Sword;
            ProductionTime = 10;
            ProductionCost = 2;
            Capacity = 5;
            Location = location;
            location.Put(this);
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
