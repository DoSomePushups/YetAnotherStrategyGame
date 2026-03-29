namespace YetAnotherStrategyGame.Domain
{
    interface IUnit : IEntity
    {
        int Damage { get; }

        UnitType Type { get; }

        int AttackCD { get; }

        int MoveCD { get; }
    }
}
