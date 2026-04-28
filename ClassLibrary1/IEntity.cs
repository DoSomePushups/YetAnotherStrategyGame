namespace Model
{

    public interface IEntityInformation
    {
        static abstract int MaxHP { get; }

        static abstract int RestTime { get; }
    }

    public interface IEntity
    {
        int HP { get; }

        int UnactionTime { get; }

        bool IsAvailable { get; }

        Cell Location { get; }

        Player Owner { get; }

        public void HandleTick();

        public void TakeDamage(int damage);

        public void Heal(int heal);

        public void Die();

        public void GetTired();

        public int GetMaxHP();
    }
}
