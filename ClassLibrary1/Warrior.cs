namespace Model
{
    public class WarriorInformation : IUnitInformation
    {
        public static int MaxHP { get; } = 150;

        public static int Damage { get; } = 80;

        public static UnitType Type { get; } = UnitType.Melee;

        public static int RestTime { get; } = 3;

    }

    public class Warrior : IUnit
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public bool IsAvailable { get; private set; }

        public EntityType Type { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Warrior(Cell location, Player owner)
        {
            HP = WarriorInformation.MaxHP / 10;
            Location = location;
            Owner = owner;
            Type = EntityType.Warrior;
            location.PutEntity(this);
        }

        public void HandleTick()
        {
            if (UnactionTime >= WarriorInformation.RestTime * 5)
            {
                IsAvailable = true;
                if (UnactionTime >= WarriorInformation.RestTime * 10)
                    Heal(Math.Max(WarriorInformation.MaxHP / 200, 1));
            }
            UnactionTime++;
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = WarriorInformation.MaxHP;
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
            if (distance != 1)
                return;
            if (entity == null)
                this.MoveTo(actionObject);
            else if (this.Owner.Team != entity.Owner.Team)
            {
                entity.TakeDamage(WarriorInformation.Damage);
                IsAvailable = false;
                UnactionTime = 0;
            }
        }

        public void GetTired()
        {
            UnactionTime = 0;
            IsAvailable = false;
        }

        public int GetMaxHP() => WarriorInformation.MaxHP;
    }
}
