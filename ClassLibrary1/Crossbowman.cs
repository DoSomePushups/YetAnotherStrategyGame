namespace Model
{
    public class CrossbowmanInformation : IRangedUnitInformation
    {
        public static int MaxHP { get; } = 100;

        public static int Range { get; } = 2;

        public static int Damage { get; } = 50;

        public static UnitType Type { get; } = UnitType.Ranged;

        public static AmmunitionType AmmoType { get; } = AmmunitionType.Arrows;

        public static int Capacity { get; } = 6;

        public static int RestTime { get; } = 2;
    }

    public class Crossbowman : IRangedUnit
    {
        public int HP { get; private set; }

        public int AmmoLeft { get; private set; }

        public int UnactionTime { get; private set; }

        public bool IsAvailable { get; private set; }

        public EntityType Type { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Crossbowman(Cell location, Player owner)
        {
            HP = CrossbowmanInformation.MaxHP / 10;
            AmmoLeft = 0;
            Location = location;
            Owner = owner;
            Type = EntityType.Crossbowman;
            location.PutEntity(this);
        }

        public void HandleTick()
        {
            if (UnactionTime >= CrossbowmanInformation.RestTime * 5)
            {
                IsAvailable = true;
                if (UnactionTime >= CrossbowmanInformation.RestTime * 10)
                    Heal(Math.Max(CrossbowmanInformation.MaxHP / 200, 1));
            }
            UnactionTime++;
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = CrossbowmanInformation.MaxHP;
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

        public void TakeAmmo(int amount) => AmmoLeft += amount;

        private void MoveTo(Cell location)
        {
            Location.RemoveEntity();
            Location = location;
            Location.PutEntity(this);
            GetTired();
        }

        public void ActUpon(Cell actionObject)
        {
            var entity = actionObject.Entity;
            var distance = Location.GetDistance(actionObject);
            if (entity == null && distance == 1)
                this.MoveTo(actionObject);
            else if (entity != null && this.Owner.Team != entity.Owner.Team && AmmoLeft > 0 && distance <= CrossbowmanInformation.Range)
            {
                AmmoLeft--;
                entity.TakeDamage(CrossbowmanInformation.Damage);
                GetTired();
            }
            else if (entity is CrossbowWorkshop crossbowWorkshop && entity.Owner.Team == this.Owner.Team)
                crossbowWorkshop.GiveAmmo(this);
        }

        public void GetTired()
        {
            UnactionTime = 0;
            IsAvailable = false;
        }

        public int GetMaxHP() => CrossbowmanInformation.MaxHP;
    }
}
