namespace Model
{
    public class HumanInformation : IUnitInformation
    {
        public static int MaxHP { get; } = 50;

        public static int Damage { get; } = 10;

        public static UnitType Type { get; } = UnitType.Melee;

        public static int AttackCD { get; } = 2;

        public static int MoveCD { get; } = 3;
    }

    public class Human : IUnit
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public bool HasMoved { get; private set; }

        public bool HasAttacked { get; private set; }

        public Human(Cell location, Player owner)
        {
            HP = 50;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = HumanInformation.MaxHP;
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
            if (distance != 1)
                return;
            if (entity == null)
                this.MoveTo(actionObject);
            else if (this.Owner.Team != actionObject.Entity.Owner.Team)
                actionObject.Entity.TakeDamage(HumanInformation.Damage);
            else if (entity.Owner.Team == actionObject.Entity.Owner.Team)
            {
                if (entity is Barracks)
                    Location.PutEntity(new Warrior(Location, Owner));
                else if (entity is CrossbowWorkshop)
                    Location.PutEntity(new Crossbowman(Location, Owner));
                else if (entity is CannonFactory)
                    Location.PutEntity(new Cannon(Location, Owner));
            }
        }
    }
}
