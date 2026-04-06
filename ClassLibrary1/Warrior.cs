namespace Model
{
    public class Warrior : IUnit
    {
        public int MaxHP { get; private set; }

        public int HP { get; private set; }

        public int Damage { get; private set; }

        public UnitType Type { get; private set; }

        public int AttackCD { get; private set; }

        public int MoveCD { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Warrior(Cell location, Player owner)
        {
            MaxHP = 150;
            HP = 150;
            Damage = 50;
            Type = UnitType.Melee;
            AttackCD = 3;
            MoveCD = 5;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            if (newHealth < MaxHP)
                HP = newHealth;
            else
                HP = MaxHP;
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
                actionObject.Entity.TakeDamage(Damage);
        }
    }
}
