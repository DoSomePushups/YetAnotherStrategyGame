namespace Model
{
    public class CannonFactory : IProductionBuilding, IAmmunitionBuilding
    {
        public int MaxHP { get; private set; }

        public int HP { get; private set; } = 1800;

        public int CostGold { get; private set; } = 80;

        public int CostFood { get; private set; } = 0;

        public int BuildTime { get; private set; } = 50;

        public Equipment EquipmentType { get; private set; } = Equipment.Cannon;

        public int ProductionTime { get; private set; } = 30;

        public int ProductionCost { get; private set; } = 15;

        public int Capacity { get; private set; } = 3;

        public int AmmoProductionTime { get; private set; } = 10;

        public int AmmoCost { get; private set; } = 5;

        public int AmmoCapacity { get; private set; } = 6;

        public AmmunitionType AmmoType { get; private set; } = AmmunitionType.Cannonballs;

        public Cell Location {  get; private set; }

        public Player Owner { get; private set; }

        public CannonFactory(Cell location, Player owner)
        {
            MaxHP = 1800;
            HP = 1800;
            CostGold = 80;
            CostFood = 0;
            BuildTime = 50;
            EquipmentType = Equipment.Cannon;
            ProductionTime = 30;
            ProductionCost = 15;
            Capacity = 3;
            AmmoProductionTime = 10;
            AmmoCost = 5;
            AmmoCapacity = 6;
            AmmoType = AmmunitionType.Cannonballs;
            Location = location;
            Owner = owner;
        }

        public void PlaceOn(Cell cell) => Location = cell;

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
