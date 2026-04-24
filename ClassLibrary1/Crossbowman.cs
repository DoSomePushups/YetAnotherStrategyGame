namespace Model
{
    public class CrossbowmanInformation : IRangedUnitInformation
    {
        public static int MaxHP { get; } = 100;

        public static int Range { get; } = 2;

        public static int Damage { get; } = 34;

        public static UnitType Type { get; } = UnitType.Ranged;

        public static AmmunitionType AmmoType { get; } = AmmunitionType.Arrows;

        public static int Capacity { get; } = 6;

        public static int AttackCD { get; } = 4;

        public static int MoveCD { get; } = 4;
    }

    public class Crossbowman : IRangedUnit
    {
        public int HP { get; private set; }

        public int AmmoLeft { get; private set; }

        public int UnactionTime { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public bool HasMoved { get; private set; }

        public bool HasAttacked { get; private set; } // Остановился здесь. Нужно делать кулдаун после хождения и атаки.

        public Crossbowman(Cell location, Player owner)
        {
            HP = 100;
            AmmoLeft = 1;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
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

        public void Die() => Location.RemoveEntity();

        private void MoveTo(Cell location)
        {
            Location.RemoveEntity();
            Location = location;
            Location.PutEntity(this);
        }

        public void ActUpon(Cell actionObject)
        {
            var entity = actionObject.Entity;
            var distance = Location.GetDistance(actionObject);
            if (entity == null && distance == 1)
                this.MoveTo(actionObject);
            else if (this.Owner.Team != actionObject.Entity.Owner.Team && AmmoLeft > 0 && distance <= CrossbowmanInformation.Range)
            {
                    AmmoLeft--;
                    actionObject.Entity.TakeDamage(CrossbowmanInformation.Damage);
            }
        }
    }
}
