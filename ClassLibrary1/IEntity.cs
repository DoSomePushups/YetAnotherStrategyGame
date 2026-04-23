namespace Model
{

    public interface IEntityInformation
    {
        static abstract int MaxHP { get; }
    }

    public interface IEntity
    {
        event Action<Cell> HpChanged;

        int HP { get; }

        int UnactionTime { get; }

        Cell Location { get; }

        Player Owner { get; }

        public void TakeDamage(int damage);

        public void Heal(int heal);

        public void Die();
    }
}
