namespace Model
{
    public class CrossbowWorkshopInformation : IProductionBuildingInformation, IAmmunitionBuildingInformation
    {
        public static int MaxHP { get; } = 1200;

        public static int CostGold { get; } = 60;

        public static int CostFood { get; } = 0;

        public static Equipment EquipmentType { get; } = Equipment.Crossbow;

        public static int RestTime { get; } = 6;

        public static int ProductionCost { get; } = 5;

        public static int Capacity { get; } = 2;

        public static int AmmoCost { get; } = 1;

        public static int AmmoCapacity { get; } = 18;

        public static AmmunitionType AmmoType { get; } = AmmunitionType.Arrows;
    }

    public class CrossbowWorkshop : IProductionBuilding, IAmmunitionBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public int ItemAmount { get; private set; }

        public int AmmoAmount { get; private set; }

        public bool IsAvailable { get; private set; }

        public EntityType Type { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public CrossbowWorkshop(Cell location, Player owner)
        {
            HP = CrossbowWorkshopInformation.MaxHP / 10;
            ItemAmount = 1;
            AmmoAmount = 6;
            Location = location;
            Owner = owner;
            Type = EntityType.CrossbowWorkshop;
            location.PutEntity(this);
        }

        public void PlaceOn(Cell cell) => Location = cell;

        public void Produce()
        {
            if (ItemAmount < CrossbowWorkshopInformation.Capacity && Owner.TryBuy(0, CrossbowWorkshopInformation.ProductionCost))
            {
                ItemAmount++;
                IsAvailable = false;
                UnactionTime = 0;
            }
        }

        public void ProduceAmmo()
        {
            var capacity = CrossbowWorkshopInformation.AmmoCapacity;
            var cost = CrossbowWorkshopInformation.AmmoCost;
            var difference = capacity - AmmoAmount;
            if (AmmoAmount < capacity &&  Owner.TryBuy(0, cost * difference))
            {
                AmmoAmount = capacity;
                GetTired();
            }
        }

        public bool TryTrain(Cell location)
        {
            if (ItemAmount > 0)
            {
                var crossbowman = new Crossbowman(location, Owner);
                GiveAmmo(crossbowman);
                location.PutEntity(crossbowman);
                Owner.GameSession.OnTick += crossbowman.HandleTick;
                Owner.OwnedEntities.Add(crossbowman);
                ItemAmount--;
                GetTired();
                return true;
            }
            return false;
        }

        public void GiveAmmo(Crossbowman unit)
        {
            var ammoTakeAmount = Math.Min(AmmoAmount, CrossbowmanInformation.Capacity - unit.AmmoLeft);
            unit.TakeAmmo(ammoTakeAmount);
            AmmoAmount -= ammoTakeAmount;
        }

        public void HandleTick()
        {
            if (UnactionTime >= CrossbowWorkshopInformation.RestTime * 5)
            {
                IsAvailable = true;
                if (UnactionTime >= CrossbowWorkshopInformation.RestTime * 10)
                    Heal((int)Math.Round(CrossbowWorkshopInformation.MaxHP / 200.0));
            }
            UnactionTime++;
        }

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

        public int GetMaxHP() => CrossbowWorkshopInformation.MaxHP;
    }
}
