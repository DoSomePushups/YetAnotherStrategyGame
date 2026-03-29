namespace YetAnotherStrategyGame.Domain
{
    class Warrior : IUnit
    {
        public int HP { get; private set; } = 150;

        public int Damage { get; private set; } = 50;

        public UnitType Type { get; private set; } = UnitType.Melee;

        public int AttackCD { get; private set; } = 3;

        public int MoveCD { get; private set; } = 5;
    }
}
