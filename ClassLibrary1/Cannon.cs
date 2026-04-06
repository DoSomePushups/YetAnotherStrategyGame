namespace Model
{
    public class Cannon : IRangedUnit
    {
        public int MaxHP { get; private set; }

        public int HP { get; private set; }

        public int Range { get; private set; }

        public int Damage { get; private set; }

        public UnitType Type { get; private set; }

        public AmmunitionType AmmoType { get; private set; }

        public int AmmoLeft { get; private set; }

        public int Capacity { get; private set; }

        public int AttackCD { get; private set; }

        public int MoveCD { get; private set; }
        
        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Cannon(Cell location, Player owner)
        {
            MaxHP = 50;
            HP = 50;
            Range = 3;
            Damage = 600;
            Type = UnitType.Ranged;
            AmmoType = AmmunitionType.Cannonballs;
            AttackCD = 15;
            MoveCD = 8;
            AmmoLeft = 0;
            Capacity = 2;
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
            if (entity == null && distance == 1)
                this.MoveTo(actionObject);
            else if (entity.Owner.Team != actionObject.Entity.Owner.Team && AmmoLeft > 0 && distance <= Range)
            {
                    AmmoLeft--;
                    actionObject.Entity.TakeDamage(Damage);
            }
        }
    }
}
