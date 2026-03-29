namespace YetAnotherStrategyGame.Domain
{
    class Cannon : IRangedUnit
    {
        public int HP { get; private set; } = 50;

        public int Range { get; private set; } = 3;

        public int Damage { get; private set; } = 600;

        public UnitType Type { get; private set; } = UnitType.Ranged;

        public AmmunitionType AmmoType { get; private set; } = AmmunitionType.Cannonballs;

        public int AttackCD { get; private set; } = 15;

        public int MoveCD { get; private set; } = 8;
    }
}
