namespace YetAnotherStrategyGame.Domain
{
    interface IRangedUnit : IUnit
    {
        int Range { get; }

        AmmunitionType AmmoType { get; }
    }
}
