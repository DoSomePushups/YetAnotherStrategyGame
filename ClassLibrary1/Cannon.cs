namespace Model
{
    public class CannonInformation : IRangedUnitInformation
    {
        public static int MaxHP { get; } = 50;

        public static int Range { get; } = 3;

        public static int Damage { get; } = 600;

        public static UnitType Type { get; } = UnitType.Ranged;

        public static AmmunitionType AmmoType { get; } = AmmunitionType.Cannonballs;

        public static int Capacity { get; } = 2;

        public static int AttackCD { get; } = 15;

        public static int MoveCD { get; } = 8;
    }

    public class Cannon : IRangedUnit
    {
        public int HP { get; private set; }

        public int AmmoLeft { get; private set; }

        public int UnactionTime { get; private set; }
        
        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public event Action<Cell> HpChanged;

        public Cannon(Cell location, Player owner)
        {
            HP = 50;
            AmmoLeft = 1;
            UnactionTime = 0;
            Location = location;
            Owner = owner;
            location.PutEntity(this);
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = CannonInformation.MaxHP;
            if (newHealth < maxHP)
                HP = newHealth;
            else
                HP = maxHP;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            HpChanged?.Invoke(Location);
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
            else if (entity != null && this.Owner.Team != actionObject.Entity.Owner.Team && AmmoLeft > 0 && distance <= CannonInformation.Range)
            {
                    AmmoLeft--;
                    actionObject.Entity.TakeDamage(CannonInformation.Damage);
            }
        }
    }
}
