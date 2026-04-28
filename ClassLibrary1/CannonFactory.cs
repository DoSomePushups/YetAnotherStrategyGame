namespace Model
{
    public class CannonFactoryInformation : IProductionBuildingInformation, IAmmunitionBuildingInformation
    {
        public static int MaxHP { get; } = 1800;

        public static int CostGold { get; } = 80;

        public static int CostFood { get; } = 0;

        public static Equipment EquipmentType { get; } = Equipment.Cannon;

        public static int RestTime { get; } = 8;

        public static int ProductionCost { get; } = 15;

        public static int Capacity { get; } = 2;

        public static int AmmoCost { get; } = 4;

        public static int AmmoCapacity { get; } = 6;

        public static AmmunitionType AmmoType { get; } = AmmunitionType.Cannonballs;
    }

    public class CannonFactory : IProductionBuilding, IAmmunitionBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public int ItemAmount { get; private set; }

        public int AmmoAmount { get; private set; }

        public bool IsAvailable { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public CannonFactory(Cell location, Player owner)
        {
            HP = CannonFactoryInformation.MaxHP / 10;
            UnactionTime = 0;
            ItemAmount = 1;
            AmmoAmount = 2;
            Location = location;
            Owner = owner;
        }

        public void Produce()
        {
            if (ItemAmount < CannonFactoryInformation.Capacity && Owner.TryBuy(0, CannonFactoryInformation.ProductionCost))
            {
                ItemAmount++;
                GetTired();
            }
        }

        public void ProduceAmmo()
        {
            var capacity = CannonFactoryInformation.AmmoCapacity;
            var cost = CannonFactoryInformation.AmmoCost;
            var difference = capacity - AmmoAmount;
            if (AmmoAmount < capacity && Owner.TryBuy(0, cost * difference))
            {
                AmmoAmount = capacity;
                GetTired();
            }
        }

        public bool TryTrain(Cell location)
        {
            if (ItemAmount > 0)
            {
                var cannon = new Cannon(location, Owner);
                GiveAmmo(cannon);
                location.PutEntity(cannon);
                ItemAmount--;
                Owner.GameSession.OnTick += () => cannon.HandleTick();
                GetTired();
                return true;
            }
            return false;
        }

        public void GiveAmmo(Cannon unit)
        {
            var ammoTakeAmount = Math.Min(AmmoAmount, CannonFactoryInformation.Capacity - unit.AmmoLeft);
            unit.TakeAmmo(ammoTakeAmount);
            AmmoAmount -= ammoTakeAmount;
        }

        public void HandleTick()
        {
            if (UnactionTime >= CannonFactoryInformation.RestTime * 5)
            {
                IsAvailable = true;
                if (UnactionTime >= CannonFactoryInformation.RestTime * 10)
                    Heal(Math.Max(CannonFactoryInformation.MaxHP / 200, 1));
            }
            UnactionTime++;
        }

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
            if (HP <= 0)
                Die();
        }

        public void Die()
        {
            Location.RemoveEntity();
            Owner.GameSession.OnTick -= () => this.HandleTick();
        }

        public void GetTired()
        {
            UnactionTime = 0;
            IsAvailable = false;
        }

        public int GetMaxHP() => CannonFactoryInformation.MaxHP;
    }
}
