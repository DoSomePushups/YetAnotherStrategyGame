namespace ClassLibrary1
{
    public class Crossbowman : IRangedUnit
    {
        public int HP { get; private set; } = 100;

        public int Range { get; private set; } = 2;

        public int Damage { get; private set; } = 34;

        public UnitType Type { get; private set; } = UnitType.Ranged;

        public AmmunitionType AmmoType { get; private set; } = AmmunitionType.Arrows;

        public int AttackCD { get; private set; } = 4;

        public int MoveCD { get; private set; } = 4;
    }
}
