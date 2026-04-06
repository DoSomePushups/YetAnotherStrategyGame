namespace Model
{
    public class Farm : IResourceBuilding
    {
        public int MaxHP { get; private set; }

        public int HP { get; private set; }

        public int CostGold { get; private set; }

        public int CostFood { get; private set; }

        public int BuildTime { get; private set; }

        public int PassiveProductionTime { get; private set; }

        public PassiveResourceType PassiveResourceType { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Farm(Cell location, Player owner)
        {
            MaxHP = 150;
            HP = 150;
            CostGold = 5;
            CostFood = 5;
            BuildTime = 15;
            PassiveProductionTime = 5;
            PassiveResourceType = PassiveResourceType.Food;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
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
