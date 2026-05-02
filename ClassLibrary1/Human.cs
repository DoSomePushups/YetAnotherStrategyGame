namespace Model
{
    public class HumanInformation : IUnitInformation
    {
        public static int MaxHP { get; } = 50;

        public static int Damage { get; } = 15;

        public static UnitType Type { get; } = UnitType.Melee;

        public static int RestTime { get; } = 1;
    }

    public class Human : IUnit
    {
        public int HP { get; private set; }

        public int UnactionTime { get; private set; }

        public bool IsAvailable { get; private set; }

        public EntityType Type { get; private set; }

        public Cell Location { get; private set; }

        public Player Owner { get; private set; }

        public Human(Cell location, Player owner)
        {
            HP = HumanInformation.MaxHP / 10;
            Location = location;
            Owner = owner;
            Type = EntityType.Human;
            location.PutEntity(this);
        }

        public void HandleTick()
        {
            if (IsAvailable && HP == HumanInformation.MaxHP)
            {
                var resourceCell = FindResourceCell(Owner.GameSession.GameField);
                if (resourceCell != null)
                    ActUpon(resourceCell);
            }
            if (UnactionTime >= HumanInformation.RestTime * 5)
            {
                IsAvailable = true;
                if (UnactionTime >= HumanInformation.RestTime * 10)
                    Heal(Math.Max(HumanInformation.MaxHP / 200, 1));
            }
            UnactionTime++;
        }

        public void Heal(int heal)
        {
            var newHealth = HP + heal;
            var maxHP = HumanInformation.MaxHP;
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
            Owner.GameSession.OnTick -= () => this.HandleTick();
            Owner.OwnedEntities.Remove(this);
        }

        private void MoveTo(Cell location)
        {
            Location.RemoveEntity();
            Location = location;
            Location.PutEntity(this);
            IsAvailable = false;
            UnactionTime = 0;
        }

        public void ActUpon(Cell actionObject)
        {
            var entity = actionObject.Entity;
            var distance = Location.GetDistance(actionObject);
            if (distance != 1)
                return;
            if (entity == null)
                this.MoveTo(actionObject);
            else if (this.Owner.Team != entity.Owner.Team)
            {
                entity.TakeDamage(HumanInformation.Damage);
                GetTired();
            }
            else if (this.Owner.Team == entity.Owner.Team)
            {
                if (entity is IProductionBuilding productionBuilding && productionBuilding.TryTrain(Location))
                    Owner.GameSession.OnTick -= () => this.HandleTick();
                else if (entity is IResourceBuilding resourceBuilding)
                {
                    resourceBuilding.Collect();
                    GetTired();
                }
            }
        }

        private Cell? FindResourceCell(Field field)
        {
            for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                {
                    var x = Location.X + i;
                    var y = Location.Y + j;
                    if (field.CheckCellExist(x, y))
                    {
                        var cell = field.Map[x, y];
                        var entity = cell.Entity;
                        if (entity is IResourceBuilding resourceBuilding && entity.Owner == this.Owner && entity.IsAvailable && entity.HP == entity.GetMaxHP())
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

        public int GetMaxHP() => HumanInformation.MaxHP;
    }
}
