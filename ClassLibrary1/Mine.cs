namespace ClassLibrary1
{
    public class Mine : IResourceBuilding
    {
        public int MaxHP { get; private set; }

        public int HP { get; private set; }

        public int CostGold { get; private set; }

        public int CostFood { get; private set; }

        public int BuildTime { get; private set; }

        public int PassiveProductionTime { get; private set; }

        public PassiveResourceType PassiveResourceType { get; private set; }

        public Cell Location { get; private set; }

        public Mine(Cell location)
        {
            MaxHP = 800;
            HP = 800;
            CostGold = 10;
            CostFood = 0;
            BuildTime = 20;
            PassiveProductionTime = 6;
            PassiveResourceType = PassiveResourceType.Gold;
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
