namespace Model
{
    public class CrossbowWorkshopInformation : IProductionBuildingInformation, IAmmunitionBuildingInformation
    {
        public static int MaxHP { get; } = 1200;

        public static int CostGold { get; } = 50;

        public static int CostFood { get; } = 0;

        public static int BuildTime { get; } = 20;

        public static Equipment EquipmentType { get; } = Equipment.Crossbow;

        public static int ProductionTime { get; } = 10;

        public static int ProductionCost { get; } = 2;

        public static int Capacity { get; } = 5;

        public static int AmmoProductionTime { get; } = 3;

        public static int AmmoCost { get; } = 2;

        public static int AmmoCapacity { get; } = 10;

        public static AmmunitionType AmmoType { get; } = AmmunitionType.Arrows;
    }

    public class CrossbowWorkshop : IProductionBuilding, IAmmunitionBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public CrossbowWorkshop(Cell location, Player owner)
        {
            HP = 1200;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
        }

        public void PlaceOn(Cell cell) => Location = cell;

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = CrossbowWorkshopInformation.MaxHP;
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
