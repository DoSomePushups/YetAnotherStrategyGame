namespace ClassLibrary1
{
    public class CrossbowWorkshop : IProductionBuilding, IAmmunitionBuilding
    {
        public int MaxHP { get; private set;  }

        public int HP { get; private set; }

        public int CostGold { get; private set; }

        public int CostFood { get; private set; }

        public int BuildTime { get; private set; }

        public Equipment EquipmentType { get; private set; }

        public int ProductionTime { get; private set; }

        public int ProductionCost { get; private set; }

        public int Capacity { get; private set; }

        public int AmmoProductionTime { get; private set; }

        public int AmmoCost { get; private set; }

        public int AmmoCapacity { get; private set; }

        public AmmunitionType AmmoType { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public CrossbowWorkshop(Cell location, Player owner)
        {
            MaxHP = 1200;
            HP = 1200;
            CostGold = 50;
            CostFood = 0;
            BuildTime = 20;
            EquipmentType = Equipment.Crossbow;
            ProductionTime = 10;
            ProductionCost = 2;
            Capacity = 5;
            AmmoProductionTime = 3;
            AmmoCost = 2;
            AmmoCapacity = 18;
            AmmoType = AmmunitionType.Arrows;
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
