namespace Model
{
    public class MineInformation : IResourceBuildingInformation
    {
        public static int MaxHP { get; } = 800;

        public static int CostGold { get; } = 15;

        public static int CostFood { get; } = 0;

        public static int RestTime { get; } = 30;

        public static int ResourceProductionQuantity { get; private set; } = 8;

        public static PassiveResourceType PassiveResourceType { get; } = PassiveResourceType.Gold;
    }

    public class Mine : IResourceBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public bool IsAvailable { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Mine(Cell location, Player owner)
        {
            HP = MineInformation.MaxHP / 10;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
        }

        public void HandleTick()
        {
            if (UnactionTime >= MineInformation.RestTime * 5)
            {
                IsAvailable = true;
                if (UnactionTime >= MineInformation.RestTime * 10)
                    Heal(Math.Max(MineInformation.MaxHP / 200, 1));
            }
            UnactionTime++;
        }

        public void Collect()
        {
            Owner.GetResources(0, MineInformation.ResourceProductionQuantity);
            GetTired();
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = MineInformation.MaxHP;
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
    }
}
