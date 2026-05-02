namespace Model
{
    public class FarmInformation : IResourceBuildingInformation
    {
        public static int MaxHP { get; } = 150;

        public static int CostGold { get; } = 5;

        public static int CostFood { get; } = 5;

        public static int RestTime { get; } = 15;

        public static int ResourceProductionQuantity { get; private set; } = 2;

        public static PassiveResourceType PassiveResourceType { get; } = PassiveResourceType.Food;
    }

    public class Farm : IResourceBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public bool IsAvailable { get; private set; }

        public EntityType Type { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Farm(Cell location, Player owner)
        {
            HP = FarmInformation.MaxHP / 10;
            Location = location;
            Owner = owner;
            Type = EntityType.Farm;
            location.PutEntity(this);
        }

        public void Collect()
        {
            Owner.GetResources(FarmInformation.ResourceProductionQuantity, 0);
            GetTired();
        }

        public void HandleTick()
        {
            if (UnactionTime >= FarmInformation.RestTime * 5)
            {
                IsAvailable = true;
                if (UnactionTime >= FarmInformation.RestTime * 6)
                    Heal(Math.Max(FarmInformation.MaxHP / 200, 1));
            }
            UnactionTime++;
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = FarmInformation.MaxHP;
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
            Owner.OwnedEntities.Remove(this);
        }

        public void GetTired()
        {
            UnactionTime = 0;
            IsAvailable = false;
        }

        public int GetMaxHP() => FarmInformation.MaxHP;
    }
}
