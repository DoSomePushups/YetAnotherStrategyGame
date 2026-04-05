namespace ClassLibrary1
{
    public interface IEntity
    {
        int MaxHP { get; }

        int HP { get; }

        Cell Location { get; }

        Player Owner { get; }

        public void TakeDamage(int damage);

        public void Heal(int heal);

        public void Die();
    }
}
