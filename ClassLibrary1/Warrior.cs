namespace Model
{
    public class WarriorInformation : IUnitInformation
    {
        public static int MaxHP { get; } = 150;

        public static int Damage { get; } = 50;

        public static UnitType Type { get; } = UnitType.Melee;

        public static int AttackCD { get; } = 3;

        public static int MoveCD { get; } = 5;

    }

    public class Warrior : IUnit
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Warrior(Cell location, Player owner)
        {
            HP = 150;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
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

        public void Die() => Location.RemoveEntity();

        private void MoveTo(Cell location) => Location = location;

        public void ActUpon(Cell actionObject)
        {
            var entity = actionObject.Entity;
            var distance = Location.GetDistance(actionObject);
            if (distance != 1)
                return;
            if (entity == null)
                this.MoveTo(actionObject);
            else if (entity.Owner.Team != actionObject.Entity.Owner.Team)
                actionObject.Entity.TakeDamage(WarriorInformation.Damage);
        }
    }
}
