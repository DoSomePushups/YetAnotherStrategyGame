namespace Model
{
    public class CastleInformation : ISpawnBuildingInformation
    {
        public static int MaxHP { get; } = 2000;

        public static int CostGold { get; } = 160;

        public static int CostFood { get; } = 20;

        public static int RestTime { get; } = 6;

        public static int SpawnCost { get; } = 10;
    }

    public class Castle : ISpawnBuilding
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public bool IsAvailable { get; private set; }

        public EntityType Type { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Castle(Cell location, Player owner)
        {
            HP = CastleInformation.MaxHP / 10;
            UnactionTime = 0;
            Location = location;
            Owner = owner;
            Type = EntityType.Castle;
        }

        public void HandleTick()
        {
            if (UnactionTime >= CastleInformation.RestTime * 5)
            {
                IsAvailable = true;
                if (UnactionTime >= CastleInformation.RestTime * 10)
                    Heal(Math.Max(CastleInformation.MaxHP / 200, 1));
            }
            UnactionTime++;
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = CastleInformation.MaxHP;
            if (newHealth < maxHP)
                HP = newHealth;
            else
                HP = maxHP;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP <= 0)
                Die();
        }

        public void Die()
        {
            Location.RemoveEntity();
            Owner.GameSession.OnTick -= this.HandleTick;
            Owner.OwnedEntities.Remove(this);
        }

        public void TrySpawn()
        {
            Cell? spawnPoint = FindSpawnCell(Owner.GameSession.GameField);
            var spawnCost = CastleInformation.SpawnCost;
            if (spawnPoint != null && Owner.TryBuy(spawnCost, 0))
            {
                var human = new Human(spawnPoint, Owner);
                spawnPoint.PutEntity(human);
                Owner.GameSession.OnTick += human.HandleTick;
                Owner.OwnedEntities.Add(human);
                GetTired();
            }
        }

        private Cell? FindSpawnCell(Field field)
        {
            for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                {
                    var x = Location.X + i;
                    var y = Location.Y + j;
                    if (field.CheckCellExist(x, y))
                    {
                        var cell = field.Map[x, y];
                        if (cell.IsEmpty)
                            return cell;
                    }
                }
            return null;
        }

        public void GetTired()
        {
            UnactionTime = 0;
            IsAvailable = false;
        }

        public int GetMaxHP() => CastleInformation.MaxHP;
    }
}
