using Model;
using NUnit.Framework;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class GameTests
    {
        private Game Game;
        [SetUp]
        public void Setup()
        {
            Game = new Game();
            Game.Start(11, 13);
        }

        [Test]
        public void DefaultGameInicialization()
        {
            var session = Game.Session;
            var firstPlayer = session.FirstPlayer;
            var secondPlayer = session.SecondPlayer;
            var field = session.GameField;
            Assert.That(Game.State, Is.EqualTo(GameState.MainScreen));

            Assert.That(session.FirstPlayer.Name, Is.EqualTo("User"));
            Assert.That(firstPlayer.Id, Is.EqualTo(1));
            Assert.That(firstPlayer.Team, Is.EqualTo(Team.First));
            Assert.That(firstPlayer.Color, Is.EqualTo(TeamColor.Blue));
            Assert.That(firstPlayer.OwnedEntities.Count == 1);
            Assert.That(firstPlayer.Food, Is.EqualTo(50));
            Assert.That(firstPlayer.Gold, Is.EqualTo(100));

            Assert.That(secondPlayer.Name, Is.EqualTo("AI"));
            Assert.That(secondPlayer.Id, Is.EqualTo(2));
            Assert.That(secondPlayer.Team, Is.EqualTo(Team.Second));
            Assert.That(secondPlayer.Color, Is.EqualTo(TeamColor.Red));
            Assert.That(secondPlayer.OwnedEntities.Count == 1);
            Assert.That(secondPlayer.Food, Is.EqualTo(50));
            Assert.That(secondPlayer.Gold, Is.EqualTo(100));

            Assert.That(field.Map[10, 12].X, Is.EqualTo(10));
            Assert.That(field.Map[10, 12].Y, Is.EqualTo(12));
            Assert.That(field.Map[10, 12].Entity, Is.Null);

            Assert.That(field.Map[5, 0].Entity is Castle);
            Assert.That(field.Map[5, 12].Entity is Castle);
            Assert.That(field.Map[5, 0].Entity.Owner, Is.EqualTo(secondPlayer));
            Assert.That(field.Map[5, 12].Entity.Owner, Is.EqualTo(firstPlayer));

            Assert.That(session.LastPlayerId, Is.EqualTo(2));
        }

        [Test]
        public void GameStateChanges()
        {
            Game.ChangeGameState(GameState.PlayOptionScreen);
            Assert.That(Game.State, Is.EqualTo(GameState.PlayOptionScreen));
            Game.ChangeGameState(GameState.GameScreen);
            Assert.That(Game.State, Is.EqualTo(GameState.GameScreen));
        }

        [Test]
        public void CanPlaceMine()
        {
            var player = Game.Session.FirstPlayer;
            var map = Game.Session.GameField.Map;
            player.SelectStoreItem(BuildingType.Mine);
            player.LeftClick(map[4, 12]);
            Assert.That(map[4, 12].Entity is Mine);
            Assert.That(map[4, 12].Entity.Owner, Is.EqualTo(player));
        }

        [Test]
        public void CanSpawnHuman()
        {
            var player = Game.Session.FirstPlayer;
            var map = Game.Session.GameField.Map;
            var castleCell = map[5, 12];
            MakeAvailable(castleCell);
            var initialFood = player.Food;
            var initialGold = player.Gold;
            player.LeftClick(castleCell);
            Assert.That(map[4, 11].Entity is Human);
            Assert.That(map[4, 11].Entity.Owner, Is.EqualTo(player));
            Assert.That(player.Food, Is.EqualTo(initialFood - CastleInformation.SpawnCost));
            Assert.That(player.Gold, Is.EqualTo(initialGold));
        }

        [Test]
        public void CanMoveHumanAfterSpawn()
        {
            var player = Game.Session.FirstPlayer;
            var map = Game.Session.GameField.Map;
            var castleCell = map[5, 12];
            var humanCell = map[4, 11];
            MakeAvailable(castleCell);
            player.LeftClick(castleCell);
            Assert.That(humanCell.Entity is Human);
            Assert.That(humanCell.Entity.Owner, Is.EqualTo(player));
            Assert.That(player.SelectedUnit == null);
            MakeAvailable(humanCell);
            player.LeftClick(humanCell);
            Assert.That(player.SelectedUnit is Human);
            player.LeftClick(map[4, 10]);
            Assert.That(humanCell.Entity == null);
            Assert.That(map[4, 10].Entity is Human);
        }

        [Test]
        public void CanPunchCastle()
        {
            var player = Game.Session.FirstPlayer;
            var map = Game.Session.GameField.Map;
            var castleCell = map[5, 12];
            var humanCell = map[4, 11];
            MakeAvailable(castleCell);
            player.LeftClick(castleCell);
            Assert.That(humanCell.Entity is Human);
            Assert.That(humanCell.Entity.Owner, Is.EqualTo(player));
            Assert.That(player.SelectedUnit == null);
            MakeAvailable(humanCell);
            player.LeftClick(humanCell);
            Assert.That(player.SelectedUnit is Human);
            var enemyCastleCell = map[4, 10];
            enemyCastleCell.PutEntity(new Castle(map[4, 10], Game.Session.SecondPlayer));
            Assert.That(enemyCastleCell.Entity is Castle);
            Assert.That(enemyCastleCell.Entity.Owner == Game.Session.SecondPlayer);
            Assert.That(enemyCastleCell.Entity.HP == CastleInformation.MaxHP / 10);
            player.LeftClick(map[4, 10]);
            Assert.That(enemyCastleCell.Entity.HP == CastleInformation.MaxHP / 10 - HumanInformation.Damage);
        }

        [Test]
        public void CanUpgradeHumanToWarrior()
        {
            var player = Game.Session.FirstPlayer;
            var map = Game.Session.GameField.Map;
            var humanCell = map[4, 11];
            var barracksCell = map[4,12];
            var castleCell = map[5, 12];
            MakeAvailable(castleCell);
            player.LeftClick(castleCell);
            Assert.That(humanCell.Entity is Human);
            Assert.That(humanCell.Entity.Owner, Is.EqualTo(player));
            Assert.That(player.SelectedUnit == null);
            player.SelectStoreItem(BuildingType.Barracks);
            player.LeftClick(barracksCell);
            Assert.That(barracksCell.Entity is Barracks);
            Assert.That(barracksCell.Entity.Owner, Is.EqualTo(player));
            MakeAvailable(humanCell);
            player.LeftClick(humanCell);
            Assert.That(player.SelectedUnit is Human);
            MakeAvailable(barracksCell);
            player.LeftClick(barracksCell);
            Assert.That(humanCell.Entity is Warrior);
        }

        [Test]
        public void CanProduceAmmo()
        {
            var player = Game.Session.FirstPlayer;
            var map = Game.Session.GameField.Map;
            var crossbowWorkshopCell = map[4, 12];
            player.SelectStoreItem(BuildingType.CrossbowWorkshop);
            player.LeftClick(crossbowWorkshopCell);
            var crossbowWorkshop = crossbowWorkshopCell.Entity as CrossbowWorkshop;
            Assert.That(crossbowWorkshop.AmmoAmount, Is.EqualTo(6));
            MakeAvailable(crossbowWorkshopCell);
            player.RightClick(crossbowWorkshopCell);
            Assert.That(crossbowWorkshop.AmmoAmount, Is.EqualTo(CrossbowWorkshopInformation.AmmoCapacity));
        }

        public void MakeAvailable(Cell cell)
        {
            var entity = cell.Entity;
            var toWait = entity switch
            {
                Warrior => WarriorInformation.RestTime,
                Crossbowman => CrossbowmanInformation.RestTime,
                Cannon => CannonInformation.RestTime,
                Human => HumanInformation.RestTime,
                Castle => CastleInformation.RestTime,
                Mine => MineInformation.RestTime,
                Farm => FarmInformation.RestTime,
                Barracks => BarracksInformation.RestTime,
                CrossbowWorkshop => CrossbowWorkshopInformation.RestTime,
                CannonFactory => CannonFactoryInformation.RestTime,
                _ => 0
            };
            for (var i = 0; i < toWait * 5 + 1; i++)
                entity.HandleTick();
        }
    }
}
