namespace Model
{
    public class CannonFactoryInformation : IProductionBuildingInformation, IAmmunitionBuildingInformation
    {
        public static int MaxHP { get; } = 1800;

        public static int CostGold { get; } = 80;

        public static int CostFood { get; } = 0;

        public static int BuildTime { get; } = 50;

        public static Equipment EquipmentType { get; } = Equipment.Cannon;

        public static int ProductionTime { get; } = 30;

        public static int ProductionCost { get; } = 15;

        public static int Capacity { get; } = 3;

        public static int AmmoProductionTime { get; } = 10;

        public static int AmmoCost { get; } = 5;

        public static int AmmoCapacity { get; } = 6;

        public static AmmunitionType AmmoType { get; } = AmmunitionType.Cannonballs;
    }

    public class CannonFactory : IProductionBuilding, IAmmunitionBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public Cell Location {  get; private set; }

        public Player Owner { get; private set; }

        public event Action<Cell> HpChanged;

        public CannonFactory(Cell location, Player owner)
        {
            HP = 1800;
            UnactionTime = 0;
            Location = location;
            Owner = owner;
        }

        public void PlaceOn(Cell cell) => Location = cell;

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = CannonFactoryInformation.MaxHP;
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
