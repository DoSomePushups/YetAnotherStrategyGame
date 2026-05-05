namespace Model
{
    public class BarracksInformation : IProductionBuildingInformation
    {
        public static int MaxHP { get; } = 1200;

        public static int CostGold { get; } = 20;

        public static int CostFood { get; } = 0;

        public static Equipment EquipmentType { get; } = Equipment.Sword;

        public static int RestTime { get; } = 5;

        public static int ProductionCost { get; } = 2;

        public static int Capacity { get; } = 5;
    }

    public class Barracks : IProductionBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public int ItemAmount { get; private set; }

        public bool IsAvailable { get; private set; }

        public EntityType Type { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Barracks(Cell location, Player owner)
        {
            HP = BarracksInformation.MaxHP / 10;
            UnactionTime = 0;
            ItemAmount = 1;
            Location = location;
            Owner = owner;
            Type = EntityType.Barracks;
        }

        public void Produce()
        {
            if (ItemAmount < BarracksInformation.Capacity && Owner.TryBuy(0, BarracksInformation.ProductionCost))
            {
                ItemAmount++;
                GetTired();
            }
        }

        public bool TryTrain(Cell location)
        {
            if (ItemAmount > 0)
            {
                var warrior = new Warrior(location, Owner);
                location.PutEntity(warrior);
                Owner.GameSession.OnTick += warrior.HandleTick;
                Owner.OwnedEntities.Add(warrior);
                ItemAmount--;
                GetTired();
                return true;
            }
            return false;
        }

        public void HandleTick()
        {
            if (UnactionTime >= BarracksInformation.RestTime * 5)
            {
                IsAvailable = true;
                if (UnactionTime >= BarracksInformation.RestTime * 10)
                    Heal(Math.Max(BarracksInformation.MaxHP / 200, 1));
            }
            UnactionTime++;
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

        public void Die()
        {
            Location.RemoveEntity();
            Owner.GameSession.OnTick -= this.HandleTick;
            Owner.OwnedEntities.Remove(this);
        }

        public void GetTired()
        {
            UnactionTime = 0;
            IsAvailable = false;
        }

        public int GetMaxHP() => BarracksInformation.MaxHP;
    }
}
